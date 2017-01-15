using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dto
{
   public class SolicitacaoDeRegistroCustasDto
    {
        public int IdSolicitacaoDeRegistroCustas { get; set; }
        public int IdSolicitacaoDeRegistro { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataSolicitacaoRegistroCustas { get; set; }

        public virtual SolicitacaoDeRegistroDto SolicitacaoDeRegistroDto { get; set; }
    }
}
