using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo
{
    [Serializable]
    public class SolicitacaoDeRegistroExigencias
    {
        [Key]
        public int IdSolicitacaoDeRegistroExigencias { get; set; }
        public int IdSolicitacaoDeRegistro { get; set; }
        public string Descricao { get; set; }
        public DateTime DataSolicitacaoRegistroExigencias { get; set; }

        public SolicitacaoDeRegistroExigencias()
        {
            DataSolicitacaoRegistroExigencias = DateTime.Now;
        }

        public virtual SolicitacaoDeRegistro SolicitacaoDeRegistro { get; set; }
    }
}
