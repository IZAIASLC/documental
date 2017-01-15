using AcessoDados;
using AcessoDados.InfraEstrutura;
using Modelo;
using Modelo.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WDocumentallApi.Core;
using WDocumentallApi.Security;

namespace WDocumentallApi.Controllers
{
    [Authorize]
    [RoutePrefix("Api/SolicitacaoDeRegistrosExigencias")]
    public class SolicitacaoDeRegistrosExigenciasController : BaseController
    {
        private IRepositorio<SolicitacaoDeRegistroExigencias> repositorio;
        private IUnitOfWork unitOfWork;
        private ILogSecurity log;
        private PaginationSet<SolicitacaoDeRegistroExigenciasDto> pageSet;
        public SolicitacaoDeRegistrosExigenciasController(IRepositorio<SolicitacaoDeRegistroExigencias> repositorio, IUnitOfWork unitOfWork, ILogSecurity log)
        {
            this.repositorio = repositorio;
            this.unitOfWork = unitOfWork;
            this.log = log;
        }

        /// <summary>
        /// Cadastrar uma solicitação de registro de exigência
        /// </summary>
        /// <param name="soliticacaoExigencias">O objeto de solicitação de registro de exigência</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Cadastrar")]
        public Task<HttpResponseMessage> Cadastrar(SolicitacaoDeRegistroExigencias soliticacaoExigencias)
        {

            if (string.IsNullOrEmpty(soliticacaoExigencias.Descricao))
                Notificacoes.Add("Campo descrição é obrigatório");

            if (soliticacaoExigencias.IdSolicitacaoDeRegistro == 0)
            {
                Notificacoes.Add("É necessário informar a solicitação de registro");
            }

            try
            {
                if (Notificacoes.Count == 0)
                {
                    repositorio.Adicionar(soliticacaoExigencias);
                    unitOfWork.Commit();

                    //Grava a transação
                    log.RegistrarLog(new LogTransacao()
                    {
                        IdTransacaoEnum = (int)Transacao.SolicitacaoDeRegistroDeExigencias,
                        IdTransacao = soliticacaoExigencias.IdSolicitacaoDeRegistroExigencias
                    });
                }

            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }

            return CreateResponse(HttpStatusCode.Created, soliticacaoExigencias);
        }

        /// <summary>
        /// Atualizar uma solicitação de registro de exigência
        /// </summary>
        /// <param name="soliticacaoExigencias">O objeto de solicitação de registro de exigência</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Atualizar")]
        public Task<HttpResponseMessage> Atualizar(SolicitacaoDeRegistroExigencias soliticacaoExigencias)
        {

            if (string.IsNullOrEmpty(soliticacaoExigencias.Descricao))
                Notificacoes.Add("Campo descrição é obrigatório");


            var exigenciasAtual = repositorio.Obter(x => x.IdSolicitacaoDeRegistroExigencias == soliticacaoExigencias.IdSolicitacaoDeRegistroExigencias).FirstOrDefault();

            exigenciasAtual.Descricao = soliticacaoExigencias.Descricao;

            try
            {
                if (Notificacoes.Count == 0)
                {
                    repositorio.Atualizar(exigenciasAtual);
                    unitOfWork.Commit();

                    //Grava a transação
                    log.RegistrarLog(new LogTransacao()
                    {
                        IdTransacaoEnum = (int)Transacao.SolicitacaoDeRegistroDeExigencias,
                        IdTransacao = soliticacaoExigencias.IdSolicitacaoDeRegistroExigencias
                    });
                }

            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }

            return CreateResponse(HttpStatusCode.Created, exigenciasAtual);
        }


        /// <summary>
        /// Listar as solicitações de registro de exigências
        /// </summary>
        /// <param name="page">A página atual</param>
        /// <param name="pageSize">O tamanho da paginação</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar/{page}/{pageSize}")]
        public Task<HttpResponseMessage> Listar(int page, int pageSize)
        {
            var soliticacaoesExigencias = new List<SolicitacaoDeRegistroExigencias>();
            try
            {
                soliticacaoesExigencias = repositorio.ObterTodos().ToList();

                if (soliticacaoesExigencias.Count == 0)
                    Notificacoes.Add("Não há registro de exigências cadastrado ");


                var currPage = page;
                var currPageSize = pageSize;

                var totalCount = soliticacaoesExigencias.Count();

                var paged = soliticacaoesExigencias.Skip(currPage * currPageSize)
                                   .Take(currPageSize)
                                   .ToArray();
 
               var retornoExigencias = new List<SolicitacaoDeRegistroExigenciasDto>();

                foreach (var exigencia in paged)
                {
                    var ex = new SolicitacaoDeRegistroExigenciasDto();

                    ex.IdSolicitacaoDeRegistroExigencias = exigencia.IdSolicitacaoDeRegistroExigencias;
                    ex.Descricao = exigencia.Descricao;
                    ex.DataSolicitacaoRegistroExigencias = exigencia.DataSolicitacaoRegistroExigencias;
                    ex.IdSolicitacaoDeRegistro = exigencia.IdSolicitacaoDeRegistro;

                    ex.SolicitacaoDeRegistroDto = new SolicitacaoDeRegistroDto();
                    ex.SolicitacaoDeRegistroDto.IdSolicitacaoDeRegistro = exigencia.SolicitacaoDeRegistro.IdSolicitacaoDeRegistro;
                    ex.SolicitacaoDeRegistroDto.Nome = exigencia.SolicitacaoDeRegistro.Nome;
                    ex.SolicitacaoDeRegistroDto.CPF = exigencia.SolicitacaoDeRegistro.CPF;
                    ex.SolicitacaoDeRegistroDto.DataSolicitacao = exigencia.SolicitacaoDeRegistro.DataSolicitacao;
                    ex.SolicitacaoDeRegistroDto.ValorDeclarado = exigencia.SolicitacaoDeRegistro.ValorDeclarado;
                    ex.SolicitacaoDeRegistroDto.QuantidadePagina = exigencia.SolicitacaoDeRegistro.QuantidadePagina;
                    ex.SolicitacaoDeRegistroDto.IDCertisign = exigencia.SolicitacaoDeRegistro.IDCertisign;
                    ex.SolicitacaoDeRegistroDto.IdStatusSolicitacao = exigencia.SolicitacaoDeRegistro.IdStatusSolicitacao;
                    retornoExigencias.Add(ex);
                }

                pageSet = new PaginationSet<SolicitacaoDeRegistroExigenciasDto>()
                {

                    Page = currPage,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / currPageSize),
                    Items = retornoExigencias

                };

            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }
            return CreateResponse(HttpStatusCode.OK, pageSet);
        }

        /// <summary>
        /// Listar uma solicitação de registro de exigências
        /// </summary>
        /// <param name="id">O identificador do objeto de solicitação de registro de exigências</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar/{id}")]
        public Task<HttpResponseMessage> Listar(int id)
        {
            var soliticacaoExigencias = new SolicitacaoDeRegistroExigencias();
            var exigencia = new SolicitacaoDeRegistroExigenciasDto();
            try
            {
                soliticacaoExigencias = repositorio.Obter(id);

                if (soliticacaoExigencias == null)
                    Notificacoes.Add("Não há registro de exigências cadastrado com o parâmetro informado ");

                exigencia.IdSolicitacaoDeRegistroExigencias = soliticacaoExigencias.IdSolicitacaoDeRegistroExigencias;
                exigencia.Descricao = soliticacaoExigencias.Descricao;
                exigencia.DataSolicitacaoRegistroExigencias = soliticacaoExigencias.DataSolicitacaoRegistroExigencias;
                exigencia.IdSolicitacaoDeRegistro = soliticacaoExigencias.IdSolicitacaoDeRegistro;

                exigencia.SolicitacaoDeRegistroDto = new SolicitacaoDeRegistroDto();
                exigencia.SolicitacaoDeRegistroDto.IdSolicitacaoDeRegistro = soliticacaoExigencias.SolicitacaoDeRegistro.IdSolicitacaoDeRegistro;
                exigencia.SolicitacaoDeRegistroDto.Nome = soliticacaoExigencias.SolicitacaoDeRegistro.Nome;
                exigencia.SolicitacaoDeRegistroDto.CPF = soliticacaoExigencias.SolicitacaoDeRegistro.CPF;
                exigencia.SolicitacaoDeRegistroDto.DataSolicitacao = soliticacaoExigencias.SolicitacaoDeRegistro.DataSolicitacao;
                exigencia.SolicitacaoDeRegistroDto.ValorDeclarado = soliticacaoExigencias.SolicitacaoDeRegistro.ValorDeclarado;
                exigencia.SolicitacaoDeRegistroDto.QuantidadePagina = soliticacaoExigencias.SolicitacaoDeRegistro.QuantidadePagina;
                exigencia.SolicitacaoDeRegistroDto.IDCertisign = soliticacaoExigencias.SolicitacaoDeRegistro.IDCertisign;
                exigencia.SolicitacaoDeRegistroDto.IdStatusSolicitacao = soliticacaoExigencias.SolicitacaoDeRegistro.IdStatusSolicitacao;

            }
            catch (Exception ex )
            {
                Notificacoes.Add(ex.Message);
            }
            return CreateResponse(HttpStatusCode.OK, exigencia);
        }

       
    }
}

