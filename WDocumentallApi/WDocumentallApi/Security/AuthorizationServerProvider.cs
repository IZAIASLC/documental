using AcessoDados;
using AcessoDados.InfraEstrutura;
using Microsoft.Owin.Security.OAuth;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WDocumentallApi.Security
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private IUsuarioApplicationService  repositorio;

      
        public AuthorizationServerProvider(IUsuarioApplicationService repositorio)
        {
            this.repositorio = repositorio;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            string encriptedSenha = StringHelper.EncriptarSenha(context.Password);

            if(!TratarArquivo.ValidarEmail(context.UserName))
            {
                context.SetError("invalid_grant", "Email inválido");
                return;
            }

            var usuario = repositorio.Autenticar(context.UserName, context.Password);
            if (usuario == null)
            {
                context.SetError("invalid_grant", "Usuário ou senha inválidos");
                return;
            }

            List<string> roles = new List<string>();
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Email));
            foreach (var perfil in usuario.UsuarioPerfils)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, perfil.Perfil.Descricao));
                roles.Add(perfil.Perfil.Descricao);
            }

            GenericPrincipal principal = new GenericPrincipal(identity, roles.ToArray());
            Thread.CurrentPrincipal = principal;

            context.Validated(identity);
        }
    }
}