using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dto
{
   public class SolicitacaoDeRegistroExigenciasDto
    {
        public int IdSolicitacaoDeRegistroExigencias { get; set; }
        public int IdSolicitacaoDeRegistro { get; set; }
        public string Descricao { get; set; }
        public DateTime DataSolicitacaoRegistroExigencias { get; set; }

        public virtual SolicitacaoDeRegistroDto SolicitacaoDeRegistroDto { get; set; }
    }
}
