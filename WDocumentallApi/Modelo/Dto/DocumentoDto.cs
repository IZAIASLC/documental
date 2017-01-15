using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dto
{
   public class DocumentoDto
    {
        public int IdDocumento { get; set; }
        public int IdSolicitacaoDeRegistro { get; set; }
        public string NomeDocumento { get; set; }
        public string DocumentoBase64 { get; set; }
        public string ExtensaoDocumento { get; set; }
        public DateTime DataCadastro { get; set; }
        
        public SolicitacaoDeRegistroDto SolicitacaoDeRegistroDto { get; set; }
    }
}
