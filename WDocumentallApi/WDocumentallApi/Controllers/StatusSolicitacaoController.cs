using AcessoDados;
using AcessoDados.InfraEstrutura;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WDocumentallApi.Security;

namespace WDocumentallApi.Controllers
{
    [Authorize]
    [RoutePrefix("Api/StatusSolicitacao")]
    public class StatusSolicitacaoController : BaseController
    {
        private IRepositorio<StatusSolicitacao> repositorio;
        private IUnitOfWork unitOfWork;
        private ILogSecurity log;
        public StatusSolicitacaoController(IRepositorio<StatusSolicitacao> repositorio, IUnitOfWork unitOfWork, ILogSecurity log)
        {
            this.repositorio = repositorio;
            this.unitOfWork = unitOfWork;
            this.log = log;
        }

        /// <summary>
        /// Listar os status da solicitação de registro
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar")]
        public Task<HttpResponseMessage> Listar()
        {
            var status = new List<StatusSolicitacao>();
            try
            {
                status = repositorio.ObterTodos().ToList();

                if (status.Count == 0)
                    Notificacoes.Add("Não há registro de status de solicitacao cadastrado ");     
            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }
            return CreateResponse(HttpStatusCode.OK, status);
        }
    }
}
