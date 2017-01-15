using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDocumentallApi.Security
{
    public interface ILogSecurity 
    {
        void RegistrarLog(LogTransacao transacao);
        Usuario ObterUsuario();
    }
}