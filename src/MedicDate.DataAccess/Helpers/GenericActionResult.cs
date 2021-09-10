using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.DataAccess.Helpers
{
    public class GenericActionResult : ActionResult
    {
        public HttpStatusCode HttpStatusCode { get; init; }
        public string ResponseBody { get; init; }

        public GenericActionResult(HttpStatusCode httpStatusCode, string responseBody)
        {
            HttpStatusCode = httpStatusCode;
            ResponseBody = responseBody;
        }

        public async override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode;
            if (!string.IsNullOrEmpty(ResponseBody))
            {
                var responseBodyBytes = Encoding.UTF8.GetBytes(ResponseBody);
                await using var ms = new MemoryStream(responseBodyBytes);
                await ms.CopyToAsync(context.HttpContext.Response.Body);
            }
        }
    }
}