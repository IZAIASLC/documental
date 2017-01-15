using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin;
using WDocumentallApi.Security;
using Modelo;
using Microsoft.Practices.Unity;
using WDocumentallApi.Dependency;

namespace WDocumentallApi
{
    public class Startup
    {
       public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            var container = new UnityContainer();
       
           ConfigureDependencyInjection(config, container);
           ConfigureWebApi(config);
        
            ConfigureOauth(app, container.Resolve<IUsuarioApplicationService>());
           
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        public void ConfigureOauth(IAppBuilder app,IUsuarioApplicationService usuarioService)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),
                Provider = new AuthorizationServerProvider(usuarioService) 
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }


        public static void ConfigureWebApi(HttpConfiguration config)
        {

            var formatters = config.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            var jsonSettings = formatters.JsonFormatter.SerializerSettings;
            jsonSettings.Formatting = Formatting.Indented;
            jsonSettings.ContractResolver = new DefaultContractResolver(); //CamelCasePropertyNamesContractResolver(); lowercase;

            formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
            config.Formatters.JsonFormatter.SerializerSettings.Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            //   config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );


            config.Routes.MapHttpRoute(
       name: "Solicitacao",
       routeTemplate: "api/{controller}/{isodata}/{status}/{page}/{pageSize}",
       defaults: new
       {
           isodata = RouteParameter.Optional,
           status = RouteParameter.Optional
       }
          );

        }

        public static void ConfigureDependencyInjection(HttpConfiguration config, UnityContainer container)
        {
            DependencyRegister.Register(container);
            config.DependencyResolver = new UnityResolverHelper(container);
        }
    }
}