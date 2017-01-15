using Modelo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WDocumentallApi
{
    public class TratarArquivo
    {
       
        public const string CONTENT_TYPE_PDF = "application/pdf";
        public const string CONTENT_TYPE_EXCEL_XLS = "application/vnd.ms-excel";
        public const string CONTENT_TYPE_EXCEL_XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";


        public static IList<SolicitacaoDeRegistro> TratarRetorno(IList<SolicitacaoDeRegistro> solicitacoes)
        {

            foreach (var solicitacao in solicitacoes)
            {
                solicitacao.Documentos.Clear();
            }

            return solicitacoes;
        }

        public static bool ValidarExtensaoDocumento(Documento documento)
        {
            var extensao = ConfigurationManager.AppSettings["extensao"].Split(',');
            if (!extensao.Contains(documento.ExtensaoDocumento))
            {
                return false;
            }

            return true;
        }

        public static bool ValidarExtensaoDocumento(SolicitacaoDeRegistro solicitacao)
        {

            var extensao = ConfigurationManager.AppSettings["extensao"].Split(',');

            foreach (var documento in solicitacao.Documentos)
            {
                if (!extensao.Contains(documento.ExtensaoDocumento))
                {
                    return false;
                    break;
                }
                  
            }

            return true;
        }

        public static string TratarDocumento(string documentoBase64)
        {

            string str = documentoBase64.Replace("data:application/pdf;base64,", " ");//pdf
            str = str.Replace("data:application/vnd.ms-excel;base64,", " ");//xls
            str = str.Replace("data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,", " ");//xlsx

            return str;
        }


        public static bool ValidarEmail(string email)
        {
            Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

            if (rg.IsMatch(email))
                return true;
            else
                return false;
        }
    }
}
 