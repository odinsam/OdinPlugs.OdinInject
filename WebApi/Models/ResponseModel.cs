using System.Net;

namespace OdinPlugs.OdinInject.WebApi.Models
{
    public class ResponseModel<T>
    {
        /// <summary>
        /// HttpStatusCode
        /// </summary>
        /// <value></value>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// Response Result
        /// </summary>
        /// <value></value>
        public T obj { get; set; }
    }
}