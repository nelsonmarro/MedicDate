using System.Net;
using System.Text.Json;

namespace MedicDate.DataAccess.Helpers
{
    public static class GenericActionResultBuilder
    {
        public static GenericActionResult BuildResult(
            HttpStatusCode statusCode, object responseBody = null)
        {
            return responseBody switch
            {
                null => new GenericActionResult(statusCode, null),
                string => new GenericActionResult(statusCode,
                    responseBody.ToString()),
                _ => new GenericActionResult(statusCode,
                    JsonSerializer.Serialize(responseBody))
            };
        }
    }
}