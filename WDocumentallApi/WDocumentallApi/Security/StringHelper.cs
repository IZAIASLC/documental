using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WDocumentallApi.Security
{
    public static class StringHelper 
    {
        public static string EncriptarSenha(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                string salt = "|54be1d80-b6d0-45c0-b8d7-13b3c798729f";
                var saltedSenha = string.Format("{0}{1}", salt, senha);
                byte[] saltedSenhaAsBytes = Encoding.UTF8.GetBytes(saltedSenha);
                return Convert.ToBase64String(sha256.ComputeHash(saltedSenhaAsBytes));
            }
        }
    }
}