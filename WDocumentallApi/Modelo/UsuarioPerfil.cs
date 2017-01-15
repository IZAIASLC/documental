using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    [Serializable]
    public class UsuarioPerfil
    {
        [Key]
        public int IdUsuarioPerfil { get; set; }
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }

        public virtual Perfil Perfil { get; set; }
    
    }
}
