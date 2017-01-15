using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    [Serializable]
    public class LogTransacao
    {
        [Key]
        public int IdLogTransacao { get; set; }    
        public int IdTransacaoEnum { get; set; }
        public int IdUsuarioTransacao { get; set; }
        public int IdTransacao { get; set; }
        public DateTime DataTransacao { get; set; }

        public LogTransacao()
        {
            DataTransacao = DateTime.Now;
        }
    }
}
