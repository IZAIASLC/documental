using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDocumentallApi.Security
{
   public interface IUsuarioApplicationService
    {
        Usuario Autenticar(string email, string senha);
    }
}