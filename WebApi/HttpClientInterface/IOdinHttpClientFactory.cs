using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OdinPlugs.OdinInject.InjectInterface;

namespace OdinPlugs.OdinInject.WebApi.HttpClientInterface
{
    public interface IOdinHttpClientFactory : IAutoInject
    {
        Task<T> GetRequestAsync<T>(string clientName, string uri, Dictionary<string, string> customHeaders = null, string mediaType = "application/json");

        Task<T> PostRequestAsync<T>(string clientName, string uri, Object obj, Dictionary<string, string> customHeaders = null, string mediaType = "application/json", Encoding encoder = null);
    }
}