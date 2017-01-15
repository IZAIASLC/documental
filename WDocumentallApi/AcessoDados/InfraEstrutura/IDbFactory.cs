using AcessoDados.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcessoDados.InfraEstrutura
{
    public interface IDbFactory : IDisposable
    {
        DocumentallCTDBEntities Init();
    }

}
