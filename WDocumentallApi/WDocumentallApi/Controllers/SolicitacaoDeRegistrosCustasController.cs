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
    [RoutePrefix("Api/SolicitacaoDeRegistrosCustas")]
    public class SolicitacaoDeRegistrosCustasController : BaseController
    {
        private IRepositorio<SolicitacaoDeRegistroCustas> repositorio;
        private IUnitOfWork unitOfWork;
        private ILogSecurity log;
        private PaginationSet<SolicitacaoDeRegistroCustasDto> pageSet;

        public SolicitacaoDeRegistrosCustasController(IRepositorio<SolicitacaoDeRegistroCustas> repositorio, IUnitOfWork unitOfWork, ILogSecurity log)
        {
            this.repositorio = repositorio;
            this.unitOfWork = unitOfWork;
            this.log = log;
        }

        /// <summary>
        /// Cadastrar uma solicitação de custas
        /// </summary>
        /// <param name="solicitacaoCustas">O objeto solicitacao de custas</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Cadastrar")]
        public Task<HttpResponseMessage> Cadastrar(SolicitacaoDeRegistroCustas solicitacaoCustas)
        {

            if (string.IsNullOrEmpty(solicitacaoCustas.Descricao))
                Notificacoes.Add("Campo descrição é obrigatório");

            if (solicitacaoCustas.Valor <= 0)
                Notificacoes.Add("Campo valor é obrigatório");

            if (solicitacaoCustas.IdSolicitacaoDeRegistro == 0)
            {
                Notificacoes.Add("É necessário informar a solicitação de registro");
            }

            try
            {

                if (Notificacoes.Count == 0)
                {
                   
                    repositorio.Adicionar(solicitacaoCustas);
                    unitOfWork.Commit();

                    //Grava a transação
                    log.RegistrarLog(new LogTransacao()
                    {
                        IdTransacaoEnum = (int)Transacao.SolicitacaoDeRegistroDeCustas,
                        IdTransacao = solicitacaoCustas.IdSolicitacaoDeRegistroCustas
                    });
                }


            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }

            return CreateResponse(HttpStatusCode.Created, solicitacaoCustas);
        }


        /// <summary>
        /// Atualizar uma solicitação de custas
        /// </summary>
        /// <param name="solicitacaoCustas">O objeto solicitacao de custas</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Atualizar")]
        public Task<HttpResponseMessage> Atualizar(SolicitacaoDeRegistroCustas solicitacaoCustas)
        {

            if (string.IsNullOrEmpty(solicitacaoCustas.Descricao))
                Notificacoes.Add("Campo descrição é obrigatório");

            if (solicitacaoCustas.Valor<=0)
                Notificacoes.Add("Campo valor é obrigatório");

            var custasAtual = repositorio.Obter(x => x.IdSolicitacaoDeRegistroCustas == solicitacaoCustas.IdSolicitacaoDeRegistroCustas).FirstOrDefault();

            custasAtual.Descricao = solicitacaoCustas.Descricao;
            custasAtual.Valor = solicitacaoCustas.Valor;
            custasAtual.IdSolicitacaoDeRegistro = solicitacaoCustas.IdSolicitacaoDeRegistro;

            try
            {

                if (Notificacoes.Count == 0)
                {
                    repositorio.Atualizar(custasAtual);
                    unitOfWork.Commit();

                    //Grava a transação
                    log.RegistrarLog(new LogTransacao()
                    {
                        IdTransacaoEnum = (int)Transacao.SolicitacaoDeRegistroDeCustas,
                        IdTransacao = solicitacaoCustas.IdSolicitacaoDeRegistroCustas
                    });
                }


            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }

            return CreateResponse(HttpStatusCode.Created, custasAtual);
        }

        /// <summary>
        /// Listar as solicitações de registro de custas
        /// </summary>
        /// <param name="page">A página atual</param>
        /// <param name="pageSize">O tamanho da paginação</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar/{page}/{pageSize}")]
        public Task<HttpResponseMessage> Listar(int page, int pageSize)
        {
            var soliticacaoesCustas = new List<SolicitacaoDeRegistroCustas>();
            try
            {
                soliticacaoesCustas = repositorio.ObterTodos().ToList();

                if (soliticacaoesCustas.Count == 0)
                    Notificacoes.Add("Não há registro de custas cadastrado ");

                var currPage = page;
                var currPageSize = pageSize;

                var totalCount = soliticacaoesCustas.Count();

                var paged = soliticacaoesCustas.Skip(currPage * currPageSize)
                                   .Take(currPageSize)
                                   .ToArray();


                var retornoCustas = new List<SolicitacaoDeRegistroCustasDto>();

                foreach (var custas in paged)
                {
                    var sc = new SolicitacaoDeRegistroCustasDto();

                    sc.IdSolicitacaoDeRegistroCustas = custas.IdSolicitacaoDeRegistroCustas;
                    sc.Descricao = custas.Descricao;
                    sc.Valor = custas.Valor;
                    sc.DataSolicitacaoRegistroCustas = custas.DataSolicitacaoRegistroCustas;
                    sc.IdSolicitacaoDeRegistro = custas.IdSolicitacaoDeRegistro;
                    sc.SolicitacaoDeRegistroDto = new SolicitacaoDeRegistroDto();
                    sc.SolicitacaoDeRegistroDto.IdSolicitacaoDeRegistro = custas.SolicitacaoDeRegistro.IdSolicitacaoDeRegistro;
                    sc.SolicitacaoDeRegistroDto.Nome = custas.SolicitacaoDeRegistro.Nome;
                    sc.SolicitacaoDeRegistroDto.CPF = custas.SolicitacaoDeRegistro.CPF;
                    sc.SolicitacaoDeRegistroDto.DataSolicitacao = custas.SolicitacaoDeRegistro.DataSolicitacao;
                    sc.SolicitacaoDeRegistroDto.ValorDeclarado = custas.SolicitacaoDeRegistro.ValorDeclarado;
                    sc.SolicitacaoDeRegistroDto.QuantidadePagina = custas.SolicitacaoDeRegistro.QuantidadePagina;
                    sc.SolicitacaoDeRegistroDto.IDCertisign = custas.SolicitacaoDeRegistro.IDCertisign;
                    sc.SolicitacaoDeRegistroDto.IdStatusSolicitacao = custas.SolicitacaoDeRegistro.IdStatusSolicitacao;
                    retornoCustas.Add(sc);
                }

                pageSet = new PaginationSet<SolicitacaoDeRegistroCustasDto>()
                {

                    Page = currPage,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / currPageSize),
                    Items = retornoCustas

                };


            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }
            return CreateResponse(HttpStatusCode.OK, pageSet);
        }

        /// <summary>
        /// Listar uma solicitação de registro de custas
        /// </summary>
        /// <param name="id">O identificador da solicitação de registro de custas</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar/{id}")]
        public Task<HttpResponseMessage> Listar(int id)
        {
            var soliticacaoesCustas = new  SolicitacaoDeRegistroCustas();
            var custas = new SolicitacaoDeRegistroCustasDto();
            try
            {
                soliticacaoesCustas = repositorio.Obter(id);

                if (soliticacaoesCustas == null)
                    Notificacoes.Add("Não há registro de custas cadastrado com o parâmetro informado");
 
                custas.IdSolicitacaoDeRegistroCustas = soliticacaoesCustas.IdSolicitacaoDeRegistroCustas;
                custas.Descricao = soliticacaoesCustas.Descricao;
                custas.Valor = soliticacaoesCustas.Valor;
                custas.DataSolicitacaoRegistroCustas = soliticacaoesCustas.DataSolicitacaoRegistroCustas;
                custas.IdSolicitacaoDeRegistro = soliticacaoesCustas.IdSolicitacaoDeRegistro;
                custas.SolicitacaoDeRegistroDto = new SolicitacaoDeRegistroDto();
                custas.SolicitacaoDeRegistroDto.IdSolicitacaoDeRegistro = soliticacaoesCustas.SolicitacaoDeRegistro.IdSolicitacaoDeRegistro;
                custas.SolicitacaoDeRegistroDto.Nome = soliticacaoesCustas.SolicitacaoDeRegistro.Nome;
                custas.SolicitacaoDeRegistroDto.CPF = soliticacaoesCustas.SolicitacaoDeRegistro.CPF;
                custas.SolicitacaoDeRegistroDto.DataSolicitacao = soliticacaoesCustas.SolicitacaoDeRegistro.DataSolicitacao;
                custas.SolicitacaoDeRegistroDto.ValorDeclarado = soliticacaoesCustas.SolicitacaoDeRegistro.ValorDeclarado;
                custas.SolicitacaoDeRegistroDto.QuantidadePagina = soliticacaoesCustas.SolicitacaoDeRegistro.QuantidadePagina;
                custas.SolicitacaoDeRegistroDto.IDCertisign = soliticacaoesCustas.SolicitacaoDeRegistro.IDCertisign;
                custas.SolicitacaoDeRegistroDto.IdStatusSolicitacao = soliticacaoesCustas.SolicitacaoDeRegistro.IdStatusSolicitacao;

            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }
            return CreateResponse(HttpStatusCode.OK, custas);
        }

       
    }
}
