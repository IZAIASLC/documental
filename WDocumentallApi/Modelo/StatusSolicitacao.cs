using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
   [Serializable]
   public class StatusSolicitacao
    {
        [Key]
        public int IdStatusSolicitacao { get; set; }
        public string Descricao { get; set; }
    }
}
