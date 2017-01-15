using AcessoDados;
using AcessoDados.InfraEstrutura;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using WDocumentallApi.Security;

namespace WDocumentallApi.Controllers
{
    [RoutePrefix("Api/Usuario")]
    public class UsuarioController : BaseController
    {
        private IRepositorio<Usuario> repositorio;
        private IUnitOfWork unitOfWork;

        public UsuarioController(IRepositorio<Usuario> repositorio, IUnitOfWork unitOfWork)
        {
            this.repositorio = repositorio;  
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Cadastrar um usuário 
        /// </summary>
        /// <param name="usuario">A entidade de usuário</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Cadastrar")]
        public Task<HttpResponseMessage> Cadastrar(Usuario usuario)
        {

            if (string.IsNullOrEmpty(usuario.Nome))
                Notificacoes.Add("Campo nome obrigatório");

            if (string.IsNullOrEmpty(usuario.Email))
                Notificacoes.Add("Campo email obrigatório");

            if (string.IsNullOrEmpty(usuario.Senha))
                Notificacoes.Add("Campo senha obrigatório");

            if (!string.IsNullOrEmpty(usuario.Email))
            {
                if (!TratarArquivo.ValidarEmail(usuario.Email))
                    Notificacoes.Add("Email inválido");
            }


            //Verifica se existe usuário com o email cadastrado
            if (repositorio.ObterTodos().Where(x => x.Email == usuario.Email).Count()>0)
            {
                Notificacoes.Add("Já existe usuário com o email cadastrado");
            }
          
            try
            {
                if (Notificacoes.Count == 0)
                {
                    usuario.Senha = StringHelper.EncriptarSenha(usuario.Senha);
                    repositorio.Adicionar(usuario);
                    unitOfWork.Commit();

                }
            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }

            return CreateResponse(HttpStatusCode.Created, null);
        }      
    }
}
