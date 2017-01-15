using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Modelo;
using AcessoDados;
using AcessoDados.InfraEstrutura;

namespace WDocumentallApi.Security
{
    public class UsuarioApplicationService : IUsuarioApplicationService
    {
        private IRepositorio<Usuario> repositorio;
        private IUnitOfWork unitOfWork;

        public UsuarioApplicationService(IRepositorio<Usuario> repositorio, IUnitOfWork unitOfWork)
        {
            this.repositorio = repositorio;
            this.unitOfWork = unitOfWork;
        }

        public Usuario Autenticar(string email, string senha)
        {
            string encriptedSenha = StringHelper.EncriptarSenha(senha);
            return repositorio.ObterTodos().Where(x => x.Email == email && x.Senha == encriptedSenha).FirstOrDefault();
        }
    }
}