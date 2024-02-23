using System.Net;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace MAModels.Models
{
    public class WebApiResponse
    {
        public static WebApiResponse Create(HttpStatusCode statusCode,
                                            object result = null,
                                            string errorMessage = null)
        {
            return new WebApiResponse(statusCode, result, errorMessage);
        }

        public string Version { get; }
        public int StatusCode { get; set; }
        public string RequestId { get; }
        public string ErrorMessage { get; set; }
        public object? Result { get; set; }
        public List<object> ResultList { get; set; }

        protected WebApiResponse(HttpStatusCode statusCode,
                                 object result = null,
                                 string errorMessage = null)
        {
            this.RequestId = Guid.NewGuid().ToString();
            this.StatusCode = (int)statusCode;
            if (result != null)
            {
                if (!isList(result))
                {
                    this.Result = result;
                    this.ResultList = new List<object> { result };
                }
                else
                {
                    this.ResultList = ((Newtonsoft.Json.Linq.JArray)result).ToObject<List<object>>();
                    this.Result = this.ResultList.FirstOrDefault();
                }
            }
            else
            {
                this.Result = null;
                this.ResultList = new List<object>();
            }
            this.ErrorMessage = errorMessage;
            this.Version = Assembly.GetEntryAssembly().GetName().Version.ToString();
        }
        private bool isList(object o)
        {
            if (o.GetType() == typeof(JObject))
                return false;
            else
                return true;
        }
    }
}