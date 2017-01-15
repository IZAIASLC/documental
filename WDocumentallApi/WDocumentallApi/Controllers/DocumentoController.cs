using AcessoDados;
using AcessoDados.InfraEstrutura;
using Modelo;
using Modelo.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using WDocumentallApi.Security;

namespace WDocumentallApi.Controllers
{
    [Authorize]
    [RoutePrefix("Api/Documento")]
    public class DocumentoController : BaseController
    {
        private IRepositorio<Documento> repositorio;
        private IUnitOfWork unitOfWork;
        private ILogSecurity log;
        public DocumentoController(IRepositorio<Documento> repositorio, IUnitOfWork unitOfWork ,ILogSecurity log)
        {
            this.repositorio = repositorio;
            this.unitOfWork = unitOfWork;
            this.log = log;
        }

        /// <summary>
        /// Anexar um documento à solicitacao
        /// </summary>
        /// <param name="documento">O objeto documento</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Anexar")]
        public Task<HttpResponseMessage> Anexar(Documento documento)
        {
            if (documento.IdSolicitacaoDeRegistro == null)
                Notificacoes.Add("É necessário informar a solicitação de registro");

            if (string.IsNullOrEmpty(documento.NomeDocumento))
                Notificacoes.Add("Documento e obrigatório");

            if (!TratarArquivo.ValidarExtensaoDocumento(documento))
                Notificacoes.Add("Somente é permitido extensão de arquivos .pdf, .xls e .xlsx");

            try
            {
                if (Notificacoes.Count == 0)
                {

                    repositorio.Adicionar(documento);
                    unitOfWork.Commit();
                }

            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }

            return CreateResponse(HttpStatusCode.Created, documento);
        }

        /// <summary>
        /// Listar os documentos pelo guid da solicitação
        /// </summary>
        /// <param name="id">O identificados da solicitação</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar/{id}")]
        public Task<HttpResponseMessage> Listar(int id)
        {
            var documentos = new List<Documento>();
            var retornoDocumentos = new List<DocumentoDto>();

            try
            {
                documentos = repositorio.ObterTodos().Where(x => x.IdSolicitacaoDeRegistro == id).ToList();

                if (documentos.Count == 0)
                    Notificacoes.Add("Não há documentos anexados para o parâmetro informado");

                foreach (var doc in documentos)
                {
                    var documento = new DocumentoDto();

                    documento.IdDocumento = doc.IdDocumento;
                    documento.NomeDocumento = doc.NomeDocumento;
                    documento.ExtensaoDocumento = doc.ExtensaoDocumento;
                    documento.DocumentoBase64 = doc.DocumentoBase64;
                    documento.DataCadastro = doc.DataCadastro;
                    documento.IdSolicitacaoDeRegistro = doc.IdSolicitacaoDeRegistro;
                    documento.SolicitacaoDeRegistroDto = new SolicitacaoDeRegistroDto();

                    documento.SolicitacaoDeRegistroDto = new SolicitacaoDeRegistroDto();
                    documento.SolicitacaoDeRegistroDto.IdSolicitacaoDeRegistro = doc.SolicitacaoDeRegistro.IdSolicitacaoDeRegistro;
                    documento.SolicitacaoDeRegistroDto.Nome = doc.SolicitacaoDeRegistro.Nome;
                    documento.SolicitacaoDeRegistroDto.CPF = doc.SolicitacaoDeRegistro.CPF;
                    documento.SolicitacaoDeRegistroDto.DataSolicitacao = doc.SolicitacaoDeRegistro.DataSolicitacao;
                    documento.SolicitacaoDeRegistroDto.ValorDeclarado = doc.SolicitacaoDeRegistro.ValorDeclarado;
                    documento.SolicitacaoDeRegistroDto.QuantidadePagina = doc.SolicitacaoDeRegistro.QuantidadePagina;
                    documento.SolicitacaoDeRegistroDto.IDCertisign = doc.SolicitacaoDeRegistro.IDCertisign;
                    documento.SolicitacaoDeRegistroDto.IdStatusSolicitacao = doc.SolicitacaoDeRegistro.IdStatusSolicitacao;
                    retornoDocumentos.Add(documento);

                }

            }
            catch (Exception ex)
            {
                Notificacoes.Add(ex.Message);
            }
            return CreateResponse(HttpStatusCode.OK, retornoDocumentos);
        }

        /// <summary>
        /// Download de um documento
        /// </summary>
        /// <param name="id">O identificador do documento</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("Download")]
        public HttpResponseMessage Download(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var applicationType = "";
                var solicitacao = repositorio.Obter(id);

                switch (solicitacao.ExtensaoDocumento)
                {
                    case "pdf":
                        applicationType = TratarArquivo.CONTENT_TYPE_PDF;
                        break;
                    case "xls":
                        applicationType = TratarArquivo.CONTENT_TYPE_EXCEL_XLS;
                        break;
                    case "xlsx":
                        applicationType = TratarArquivo.CONTENT_TYPE_EXCEL_XLSX;
                        break;
                }

                byte[] documento = Convert.FromBase64String(TratarArquivo.TratarDocumento(solicitacao.DocumentoBase64));

                MemoryStream ms = new MemoryStream(documento);
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StreamContent(ms);
                response.Content.Headers.Add("x-filename", solicitacao.NomeDocumento);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");// { FileName = solicitacao.NomeDocumento };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(applicationType);
                response.Content.Headers.ContentDisposition.FileName = solicitacao.NomeDocumento;

                return response;
            }

            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;

            }
        }
        /// <summary>
        /// Visualizar um documento
        /// </summary>
        /// <param name="id">O identificador do documento</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("Visualizar")]
        public HttpResponseMessage Visualizar(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var applicationType = "";
                var solicitacao = repositorio.Obter(id);

                switch (solicitacao.ExtensaoDocumento)
                {
                    case "pdf":
                        applicationType = TratarArquivo.CONTENT_TYPE_PDF;
                        break;
                    case "xls":
                        applicationType = TratarArquivo.CONTENT_TYPE_EXCEL_XLS;
                        break;
                    case "xlsx":
                        applicationType = TratarArquivo.CONTENT_TYPE_EXCEL_XLSX;
                        break;
                }

                byte[] documento = Convert.FromBase64String(TratarArquivo.TratarDocumento(solicitacao.DocumentoBase64));

                MemoryStream ms = new MemoryStream(documento);
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StreamContent(ms);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(applicationType);

                return response;
            }

            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;

            }

        }
    }

}
