using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    [Serializable]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }

        public Usuario()
        {
           DataCadastro = DateTime.Now;
            UsuarioPerfils = new List<UsuarioPerfil>();
        }

        public virtual ICollection<UsuarioPerfil> UsuarioPerfils { get; set; }
    }
}
