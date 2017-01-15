using AcessoDados.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcessoDados.InfraEstrutura
{
    public class DbFactory : Disposable, IDbFactory
    {
        DocumentallCTDBEntities dbContext;

        public DocumentallCTDBEntities Init()
        {
            return dbContext ?? (dbContext = new DocumentallCTDBEntities());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
