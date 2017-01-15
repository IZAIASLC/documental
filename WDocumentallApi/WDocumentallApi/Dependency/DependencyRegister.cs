using AcessoDados;
using AcessoDados.Contexto;
using AcessoDados.InfraEstrutura;
using Microsoft.Practices.Unity;
using WDocumentallApi.Security;

namespace WDocumentallApi.Dependency
{
    public static class DependencyRegister
    {
        /// <summary>
        /// TransientLifetimeManager - Cada Resolve gera uma nova instância.
        /// ContainerControlledLifetimeManager - Utiliza Singleton
        /// </summary>
        /// <param name="container"></param>
        public static void Register(UnityContainer container)
        {
            container.RegisterType<DocumentallCTDBEntities, DocumentallCTDBEntities>(new HierarchicalLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IDbFactory, DbFactory>(new HierarchicalLifetimeManager());
            container.RegisterType(typeof(IRepositorio<>), typeof(Repositorio<>));
            container.RegisterType<IUsuarioApplicationService, UsuarioApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILogSecurity, LogSecurity>(new HierarchicalLifetimeManager());     
        }
    }
}