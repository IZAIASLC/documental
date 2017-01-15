using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Modelo
{
   [Serializable]
    public class Perfil
    {
        [Key]
        public int IdPerfil { get; set; }
        public string Descricao { get; set; }
    }
}
