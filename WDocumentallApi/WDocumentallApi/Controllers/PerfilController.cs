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
     
    [RoutePrefix("Api/Perfil")]
    public class PerfilController : BaseController
    {
        private IRepositorio<Perfil> repositorio;      
        private IUnitOfWork unitOfWork;

        public PerfilController(IRepositorio<Perfil> repositorio, IUnitOfWork unitOfWork)
        {
            this.repositorio = repositorio;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Listar os perfils
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar")]
        public Task<HttpResponseMessage> Listar()
        {
            var perfils = new List<Perfil>();
            try
            {
                perfils = repositorio.ObterTodos().ToList();

                if (perfils.Count == 0)
                    Notificacoes.Add("Não há perfil cadastrado");
            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }
            return CreateResponse(HttpStatusCode.OK, perfils);
        }
    }
}