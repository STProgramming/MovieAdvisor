using System.Net;
using MAModels.Models;
using Newtonsoft.Json;

namespace MAApi.Middlewares
{
    public class WebApiResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public WebApiResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var currentBody = context.Response.Body;
            string errorMessage = null;

            using (var memoryStream = new MemoryStream())
            {

                context.Response.Body = memoryStream;

                await _next(context);

                if (context.Response.StatusCode != 204)
                {
                    //reset the body 
                    context.Response.Body = currentBody;
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var readToEnd = new StreamReader(memoryStream).ReadToEnd();
                    object objResult;
                    try
                    {
                        objResult = JsonConvert.DeserializeObject(readToEnd);
                    }
                    catch (Exception ex)
                    {
                        objResult = null;
                        errorMessage = readToEnd;
                    }
                    var result = WebApiResponse.Create((HttpStatusCode)context.Response.StatusCode,
                                                    objResult,
                                                    errorMessage);
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                }
            }
        }
    }
}
