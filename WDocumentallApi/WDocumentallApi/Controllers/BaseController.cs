
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WDocumentallApi.Security;

namespace WDocumentallApi.Controllers
{
   
    public class BaseController : ApiController
    {   
        public List<string>   Notificacoes;
        public HttpResponseMessage ResponseMessage;   
        public BaseController()
        {
            this.Notificacoes = new List<string>(); 
            this.ResponseMessage = new HttpResponseMessage();
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public Task<HttpResponseMessage> CreateResponse(HttpStatusCode code, object result)
        {
 
            if (Notificacoes.Count > 0)
               
                ResponseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, new { errors = Notificacoes });
           
            else
                ResponseMessage = Request.CreateResponse(code, result);

            return Task.FromResult<HttpResponseMessage>(ResponseMessage);
        }    
    }
}