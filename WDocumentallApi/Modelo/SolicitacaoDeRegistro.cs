using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Modelo
{
    [Serializable]
    public class SolicitacaoDeRegistro
    {

        public SolicitacaoDeRegistro()
        {
            Documentos = new List<Documento>();
            SolicitacaoDeRegistrosCustas = new List<SolicitacaoDeRegistroCustas>();
            SolicitacaoDeRegistrosExigencias = new List<SolicitacaoDeRegistroExigencias>();
            DataSolicitacao = DateTime.Now;
        }
    
        [Key]
        public int IdSolicitacaoDeRegistro { get; set; }
        public string Nome { get; set; }
        public int CPF { get; set; }    
        public DateTime DataSolicitacao { get; set; }
        public Nullable<decimal> ValorDeclarado { get; set; }
        public Nullable<int> QuantidadePagina { get; set; }
        public string IDCertisign { get; set; }
        public int IdStatusSolicitacao { get; set; }

        public virtual StatusSolicitacao StatusSolicitacao { get; set; }
        public virtual ICollection<Documento> Documentos { get; set; }
        public virtual ICollection<SolicitacaoDeRegistroCustas> SolicitacaoDeRegistrosCustas { get; set; }       
        public virtual ICollection<SolicitacaoDeRegistroExigencias> SolicitacaoDeRegistrosExigencias { get; set; }

      
    }
}