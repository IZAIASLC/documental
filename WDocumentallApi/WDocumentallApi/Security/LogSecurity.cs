using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Modelo;
using AcessoDados;
using AcessoDados.InfraEstrutura;
using System.Threading;

namespace WDocumentallApi.Security
{
    public class LogSecurity  : ILogSecurity        
    {
        private IRepositorio<LogTransacao> repositorio;
        private IRepositorio<Usuario> repositorioUsuario;
        private IUnitOfWork unitOfWork;

        public LogSecurity(IRepositorio<LogTransacao> repositorio, IUnitOfWork unitOfWork, IRepositorio<Usuario> repositorioUsuario)
        {
            this.repositorio = repositorio;
            this.unitOfWork = unitOfWork;
            this.repositorioUsuario = repositorioUsuario;
        }

        public void RegistrarLog(LogTransacao transacao)
        {
            transacao.IdUsuarioTransacao = ObterUsuario().IdUsuario;
            repositorio.Adicionar(transacao);
            unitOfWork.Commit();
        }

        public Usuario ObterUsuario()
        {
            var email = Thread.CurrentPrincipal.Identity.Name;
            return repositorioUsuario.ObterTodos().Where(u => u.Email == email).FirstOrDefault();
        }
    }
}