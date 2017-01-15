using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo
{
    [Serializable]
    public class SolicitacaoDeRegistroCustas
    {
        [Key]
        public int IdSolicitacaoDeRegistroCustas { get; set; }
        public int IdSolicitacaoDeRegistro { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataSolicitacaoRegistroCustas { get; set; }

        public SolicitacaoDeRegistroCustas()
        {
            DataSolicitacaoRegistroCustas = DateTime.Now;
        }
        public virtual SolicitacaoDeRegistro SolicitacaoDeRegistro { get; set; }
    }
}
