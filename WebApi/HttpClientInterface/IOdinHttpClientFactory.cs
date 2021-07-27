using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OdinPlugs.OdinInject.InjectInterface;
using OdinPlugs.OdinInject.WebApi.Models;

namespace OdinPlugs.OdinInject.WebApi.HttpClientInterface
{
    public interface IOdinHttpClientFactory : IAutoInject
    {
        /// <summary>
        /// GetHttpRequestAsync
        /// </summary>
        /// <param name="clientName">httpClient Name</param>
        /// <param name="uri">request uri</param>
        /// <param name="accessToken">request access Token</param>
        /// <param name="customHeaders">request headers</param>
        /// <param name="mediaType">request mediaType</param>
        /// <typeparam name="T">result obj Type</typeparam>
        /// <returns>type of Result</returns>
        Task<ResponseModel<T>> GetHttpRequestAsync<T>(string clientName, string uri, string accessToken = null, Dictionary<string, string> customHeaders = null, string mediaType = "application/json");

        /// <summary>
        /// GetRequestAsync
        /// </summary>
        /// <param name="clientName">httpClient Name</param>
        /// <param name="uri">request uri</param>
        /// <param name="accessToken">request access Token</param>
        /// <param name="customHeaders">request headers</param>
        /// <param name="mediaType">request mediaType</param>
        /// <typeparam name="T">result obj Type</typeparam>
        /// <returns>type of Result</returns>
        [Obsolete("this method is Obsolete.please use GetHttpRequestAsync<T>")]
        Task<T> GetRequestAsync<T>(string clientName, string uri, string accessToken = null, Dictionary<string, string> customHeaders = null, string mediaType = "application/json");

        /// <summary>
        /// PostRequestAsync
        /// </summary>
        /// <param name="clientName">httpClient Name</param>
        /// <param name="uri">request uri</param>
        /// <param name="obj">request body</param>
        /// <param name="customHeaders">request headers</param>
        /// <param name="mediaType">request mediaType</param>
        /// <param name="encoder">request encoder</param>
        /// <typeparam name="T">result Type</typeparam>
        /// <returns>type of Result</returns>

        /// <summary>
        /// PostRequestAsync
        /// </summary>
        /// <param name="clientName">httpClient Name</param>
        /// <param name="uri">request uri</param>
        /// <param name="obj">request body</param>
        /// <param name="accessToken">request access Token</param>
        /// <param name="customHeaders">request headers</param>
        /// <param name="mediaType">request mediaType</param>
        /// <param name="encoder">result Type</param>
        /// <typeparam name="T">result Type</typeparam>
        /// <returns></returns>
        [Obsolete("this method is Obsolete.please use PostHttpRequestAsync<T>")]
        Task<T> PostRequestAsync<T>(string clientName, string uri, Object obj, string accessToken = null, Dictionary<string, string> customHeaders = null, string mediaType = "application/json", Encoding encoder = null);


        /// <summary>
        /// PostHttpRequestAsync
        /// </summary>
        /// <param name="clientName">httpClient Name</param>
        /// <param name="uri">request uri</param>
        /// <param name="obj">request body</param>
        /// <param name="accessToken">request access Token</param>
        /// <param name="customHeaders">request headers</param>
        /// <param name="mediaType">request mediaType</param>
        /// <param name="encoder">result Type</param>
        /// <typeparam name="T">result Type</typeparam>
        /// <returns></returns>
        Task<ResponseModel<T>> PostHttpRequestAsync<T>(string clientName, string uri, Object obj, string accessToken = null, Dictionary<string, string> customHeaders = null, string mediaType = "application/json", Encoding encoder = null);
    }
}