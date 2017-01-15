using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    [Serializable]
    public class Documento
    {
        [Key]
        public int IdDocumento { get; set; }
        public int IdSolicitacaoDeRegistro { get; set; }
        public string NomeDocumento { get; set; }
        public string DocumentoBase64 { get; set; }    
        public string ExtensaoDocumento { get; set; }
        public DateTime DataCadastro { get; set; }
        public Documento()
        {
            DataCadastro = DateTime.Now;
        }

        public virtual SolicitacaoDeRegistro SolicitacaoDeRegistro { get; set; }
    }
}
