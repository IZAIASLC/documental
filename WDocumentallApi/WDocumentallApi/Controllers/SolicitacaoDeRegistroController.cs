using AcessoDados;
using AcessoDados.Contexto;
using AcessoDados.InfraEstrutura;
using Modelo;
using Modelo.Dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using WDocumentallApi.Core;
using WDocumentallApi.Security;

namespace WDocumentallApi.Controllers
{
    [Authorize(Roles ="Administrador")]
    [RoutePrefix("Api/SolicitacaoDeRegistro")]
    public class SolicitacaoDeRegistroController : BaseController
    {
        private IRepositorio<SolicitacaoDeRegistro> repositorio;
        private IRepositorio<StatusSolicitacao> repositorioStatusSolicitacao;
        private IRepositorio<Documento> repositorioDocumento;
        private IUnitOfWork unitOfWork;
        private ILogSecurity log;
        private PaginationSet<SolicitacaoDeRegistroDto> pageSet;
        public SolicitacaoDeRegistroController(IRepositorio<SolicitacaoDeRegistro> repositorio,IRepositorio<StatusSolicitacao> repositorioStatusSolicitacao,IRepositorio<Documento> repositorioDocumento, IUnitOfWork unitOfWork,ILogSecurity log)
        {
            this.repositorio = repositorio;
            this.repositorioStatusSolicitacao = repositorioStatusSolicitacao;
            this.repositorioDocumento = repositorioDocumento;
            this.unitOfWork = unitOfWork;
            this.log = log;
        }

        /// <summary>
        /// Cadastrar uma solicitação de registro
        /// </summary>
        /// <param name="solicitacao">O objeto solicitação</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Cadastrar")]
        public Task<HttpResponseMessage> Cadastrar(SolicitacaoDeRegistro solicitacao)
        {

            solicitacao.IdStatusSolicitacao = repositorioStatusSolicitacao.ObterTodos().Where(x => x.Descricao.ToUpper() == "Enviado").FirstOrDefault().IdStatusSolicitacao;

            if (string.IsNullOrEmpty(solicitacao.Nome))
                Notificacoes.Add("Campo nome é obrigatório");

            if (string.IsNullOrEmpty(solicitacao.CPF.ToString()))
                Notificacoes.Add("Campo CPF é obrigatório");

            if(string.IsNullOrEmpty(solicitacao.IDCertisign))
                Notificacoes.Add("Campo ID Certisign é obrigatório");

            
            if (solicitacao.Documentos.Count == 0)
                Notificacoes.Add("É necessário anexar um documento");

            if (!TratarArquivo.ValidarExtensaoDocumento(solicitacao))
                Notificacoes.Add("Somente é permitido extensão de arquivos .pdf, .xls e .xlsx");

            try
            {
                if (Notificacoes.Count == 0)
                {
                    repositorio.Adicionar(solicitacao);
                    unitOfWork.Commit();

                    //Grava a transação
                    log.RegistrarLog(new LogTransacao() 
                    { 
                        IdTransacaoEnum=(int)Transacao.SolicitacaoDeRegistro,
                        IdTransacao=solicitacao.IdSolicitacaoDeRegistro
                    });
                }

            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }

            return CreateResponse(HttpStatusCode.Created, solicitacao);
        }

        /// <summary>
        /// Atualizar uma solicitação de registro
        /// </summary>
        /// <param name="solicitacao">O objeto solicitação</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Atualizar")]
        public Task<HttpResponseMessage> Atualizar(SolicitacaoDeRegistro solicitacao)
        {

            solicitacao.IdStatusSolicitacao = repositorioStatusSolicitacao.ObterTodos().Where(x => x.Descricao.ToUpper() == "Enviado").FirstOrDefault().IdStatusSolicitacao;

            if (string.IsNullOrEmpty(solicitacao.Nome))
                Notificacoes.Add("Campo nome é obrigatório");

            if (string.IsNullOrEmpty(solicitacao.CPF.ToString()))
                Notificacoes.Add("Campo CPF é obrigatório");

            if (string.IsNullOrEmpty(solicitacao.IDCertisign))
                Notificacoes.Add("Campo ID Certisign é obrigatório");

          
            if (solicitacao.Documentos.Count == 0)
                Notificacoes.Add("É necessário anexar um documento");

            if (!TratarArquivo.ValidarExtensaoDocumento(solicitacao))
                Notificacoes.Add("Somente é permitido extensão de arquivos .pdf, .xls e .xlsx");


           var solicitacaoAtual = repositorio.Obter(x => x.IdSolicitacaoDeRegistro == solicitacao.IdSolicitacaoDeRegistro).FirstOrDefault();

            solicitacaoAtual.Nome = solicitacao.Nome;
            solicitacaoAtual.CPF = solicitacao.CPF;
            solicitacaoAtual.ValorDeclarado = solicitacao.ValorDeclarado;
            solicitacaoAtual.QuantidadePagina = solicitacao.QuantidadePagina;
            solicitacaoAtual.IDCertisign = solicitacao.IDCertisign;

            foreach (var documento in solicitacao.Documentos)
            {

                if (solicitacaoAtual.Documentos.Where(x => x.IdDocumento == documento.IdDocumento).Count() == 0)
                    solicitacaoAtual.Documentos.Add(documento);
            }

            try
            {
                if (Notificacoes.Count == 0)
                {
                    repositorio.Atualizar(solicitacaoAtual);
                    unitOfWork.Commit();

                    //Grava a transação
                    log.RegistrarLog(new LogTransacao()
                    {
                        IdTransacaoEnum = (int)Transacao.AlterarSolicitacaoDeRegistro,
                        IdTransacao = solicitacao.IdSolicitacaoDeRegistro
                    });
                }

            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }

            return CreateResponse(HttpStatusCode.Created, solicitacaoAtual);
        }


        /// <summary>
        /// Atualizar o status de uma solicitação de registro
        /// </summary>
        /// <param name="solicitacao">O objeto solicitação</param>
        /// <returns></returns>
        [HttpPut]
        [Route("Atualizar-Status")]
        public Task<HttpResponseMessage> AtualizarStatus(SolicitacaoDeRegistro solicitacao)
        {

            if (solicitacao.StatusSolicitacao == null)
                Notificacoes.Add("Campo status da solicitação é obrigatório");

            try
            {

                if (Notificacoes.Count == 0)
                {
                    var solicitacaoAtual = repositorio.Obter(solicitacao.IdSolicitacaoDeRegistro);

                    solicitacaoAtual.StatusSolicitacao = solicitacao.StatusSolicitacao;
                    repositorio.Atualizar(solicitacaoAtual);
                     unitOfWork.Commit();
                    //Grava a transação
                    log.RegistrarLog(new LogTransacao()
                    {
                        IdTransacaoEnum = (int)Transacao.AlterarStatusSolicitacaoRegistro,
                        IdTransacao = solicitacaoAtual.IdSolicitacaoDeRegistro
                    });

                }

            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }

            return CreateResponse(HttpStatusCode.OK, solicitacao);
        }

        /// <summary>
        /// Listar uma solicitação pelo guid
        /// </summary>
        /// <param name="id">O identificador da solicitação</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar/{id}")]
        public Task<HttpResponseMessage> Listar(int id)
        {

            var soliticacao = new SolicitacaoDeRegistro();
            var solicitacaoRetorno = new SolicitacaoDeRegistroDto();
            try
            {
                soliticacao = repositorio.Obter(id);

                if (soliticacao == null)
                    Notificacoes.Add("Não há registro de solicitação cadastrado com o parâmetro informado ");

                solicitacaoRetorno = new SolicitacaoDeRegistroDto();
                solicitacaoRetorno.IdSolicitacaoDeRegistro = soliticacao.IdSolicitacaoDeRegistro;
                solicitacaoRetorno.Nome = soliticacao.Nome;
                solicitacaoRetorno.CPF = soliticacao.CPF;
                solicitacaoRetorno.DataSolicitacao = soliticacao.DataSolicitacao;
                solicitacaoRetorno.ValorDeclarado = soliticacao.ValorDeclarado;
                solicitacaoRetorno.QuantidadePagina = soliticacao.QuantidadePagina;
                solicitacaoRetorno.IDCertisign = soliticacao.IDCertisign;
                solicitacaoRetorno.IdStatusSolicitacao = soliticacao.IdStatusSolicitacao;
                solicitacaoRetorno.Documentos = new List<DocumentoDto>();
                foreach (var doc in soliticacao.Documentos)
                {
                    var documento = new DocumentoDto();
                    documento.IdDocumento = doc.IdDocumento;
                    documento.NomeDocumento = doc.NomeDocumento;
                    documento.ExtensaoDocumento = doc.ExtensaoDocumento;
                    documento.DocumentoBase64 = doc.DocumentoBase64;
                    documento.DataCadastro = doc.DataCadastro;
                    documento.IdSolicitacaoDeRegistro = doc.IdSolicitacaoDeRegistro;
                    solicitacaoRetorno.Documentos.Add(documento);

                }


            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }
            return CreateResponse(HttpStatusCode.OK, solicitacaoRetorno);
        }

        /// <summary>
        /// Listar as solicitações de registro
        /// </summary>
        /// <param name="page">A página atual</param>
        /// <param name="pageSize">O tamanho da paginação</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar/{page}/{pageSize}")]
        public Task<HttpResponseMessage> Listar(int page, int pageSize)
        {
            var solicitacoesRetorno = new List<SolicitacaoDeRegistro>();
          
            try
            {
                var soliticacaoes = repositorio.ObterTodos().ToList();

                if (soliticacaoes.Count == 0)
                    Notificacoes.Add("Não há registro de solicitação cadastrado ");

                solicitacoesRetorno = TratarArquivo.TratarRetorno(soliticacaoes).ToList();


                var currPage = page;
                var currPageSize = pageSize;

                var totalCount = solicitacoesRetorno.Count();

                var paged = solicitacoesRetorno.Skip(currPage * currPageSize)
                                   .Take(currPageSize)
                                   .ToArray();

                var solicitacoesRetornoDto = new List<SolicitacaoDeRegistroDto>();

                foreach (var solicit in paged)
                {
                  var solicitacaoRetorno = new SolicitacaoDeRegistroDto();
                    solicitacaoRetorno.IdSolicitacaoDeRegistro = solicit.IdSolicitacaoDeRegistro;
                    solicitacaoRetorno.Nome = solicit.Nome;
                    solicitacaoRetorno.CPF = solicit.CPF;
                    solicitacaoRetorno.DataSolicitacao = solicit.DataSolicitacao;
                    solicitacaoRetorno.ValorDeclarado = solicit.ValorDeclarado;
                    solicitacaoRetorno.QuantidadePagina = solicit.QuantidadePagina;
                    solicitacaoRetorno.IDCertisign = solicit.IDCertisign;
                    solicitacaoRetorno.IdStatusSolicitacao = solicit.IdStatusSolicitacao;

                    solicitacaoRetorno.Documentos = new List<DocumentoDto>();
                    foreach (var doc in solicit.Documentos)
                    {
                        var documento = new DocumentoDto();
                        documento.IdDocumento = doc.IdDocumento;
                        documento.NomeDocumento = doc.NomeDocumento;
                        documento.ExtensaoDocumento = doc.ExtensaoDocumento;
                        documento.DocumentoBase64 = doc.DocumentoBase64;
                        documento.DataCadastro = doc.DataCadastro;
                        documento.IdSolicitacaoDeRegistro = doc.IdSolicitacaoDeRegistro;
                        solicitacaoRetorno.Documentos.Add(documento);
                    }
                    solicitacoesRetornoDto.Add(solicitacaoRetorno);
                }

                pageSet = new PaginationSet<SolicitacaoDeRegistroDto>()
                {

                    Page = currPage,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / currPageSize),
                    Items = solicitacoesRetornoDto

                };
            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }

            return CreateResponse(HttpStatusCode.OK, pageSet);
        }

        /// <summary>
        /// Pesquisar a solicitação de registro. Usar o formato iso-date 07-01-2017
        /// </summary>
        /// <param name="isodata">A data no formato iso {01-01-2017}</param>
        /// <param name="status">O status da solicitação</param>
        /// <param name="page">A página atual</param>
        /// <param name="pageSize">O tamanho da paginação</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Pesquisar/{isodata?}/{status?}/{page}/{pageSize}")]
        public Task<HttpResponseMessage> Pesquisar(string isodata, int? status,int page, int pageSize)
        {
            var solicitacaoAtual = new List<SolicitacaoDeRegistro>();
            var soliticacao= new List<SolicitacaoDeRegistro>();
        
            try
            {
                DateTime? data = null;

                if (isodata != null)
                    data = Convert.ToDateTime(isodata);
                

                if (isodata!=null || status.HasValue)
                {
                    solicitacaoAtual = repositorio.ObterTodos().ToList();

                    
                    if (isodata != null)
                        solicitacaoAtual = solicitacaoAtual.Where(x => x.DataSolicitacao.ToShortDateString() == data.Value.ToShortDateString()).ToList();

                    if(status.HasValue)
                        solicitacaoAtual = solicitacaoAtual.Where(x => x.IdStatusSolicitacao == status.Value).ToList();
                }
               else
                    Notificacoes.Add("Informe um parâmetro para realizar a pesquisa");

                soliticacao = TratarArquivo.TratarRetorno(solicitacaoAtual).ToList();

                if (soliticacao.Count == 0)
                    Notificacoes.Add("Não há registro de solicitação cadastrado com o parâmetro informado ");


                var currPage = page;
                var currPageSize = pageSize;

                var totalCount = soliticacao.Count();

                var paged = soliticacao.Skip(currPage * currPageSize)
                                   .Take(currPageSize)
                                   .ToArray();

                var solicitacoesRetornoDto = new List<SolicitacaoDeRegistroDto>();

                foreach (var solicit in paged)
                {
                    var solicitacaoRetorno = new SolicitacaoDeRegistroDto();
                    solicitacaoRetorno.IdSolicitacaoDeRegistro = solicit.IdSolicitacaoDeRegistro;
                    solicitacaoRetorno.Nome = solicit.Nome;
                    solicitacaoRetorno.CPF = solicit.CPF;
                    solicitacaoRetorno.DataSolicitacao = solicit.DataSolicitacao;
                    solicitacaoRetorno.ValorDeclarado = solicit.ValorDeclarado;
                    solicitacaoRetorno.QuantidadePagina = solicit.QuantidadePagina;
                    solicitacaoRetorno.IDCertisign = solicit.IDCertisign;
                    solicitacaoRetorno.IdStatusSolicitacao = solicit.IdStatusSolicitacao;

                    solicitacaoRetorno.Documentos = new List<DocumentoDto>();
                    foreach (var doc in solicit.Documentos)
                    {
                        var documento = new DocumentoDto();
                        documento.IdDocumento = doc.IdDocumento;
                        documento.NomeDocumento = doc.NomeDocumento;
                        documento.ExtensaoDocumento = doc.ExtensaoDocumento;
                        documento.DocumentoBase64 = doc.DocumentoBase64;
                        documento.DataCadastro = doc.DataCadastro;
                        documento.IdSolicitacaoDeRegistro = doc.IdSolicitacaoDeRegistro;
                        solicitacaoRetorno.Documentos.Add(documento);
                    }
                    solicitacoesRetornoDto.Add(solicitacaoRetorno);
                }

                pageSet = new PaginationSet<SolicitacaoDeRegistroDto>()
                {

                    Page = currPage,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / currPageSize),
                    Items = solicitacoesRetornoDto

                };


            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }
            return CreateResponse(HttpStatusCode.OK, pageSet);
        }

       
    }
}
 