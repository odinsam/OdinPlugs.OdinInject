using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OdinPlugs.OdinInject.InjectCore;
using OdinPlugs.OdinInject.WebApi.HttpClientInterface;
using OdinPlugs.OdinUtils.OdinExtensions.BasicExtensions.OdinString;
using IdentityModel.Client;
namespace OdinPlugs.OdinInject.WebApi
{
    public class OdinHttpClientFactory : IOdinHttpClientFactory
    {
        public async Task<T> GetRequestAsync<T>(string clientName, string uri, string accessToken = null, Dictionary<string, string> customHeaders = null, string mediaType = "application/json")
        {
            var clientFactory = OdinInjectCore.GetService<IHttpClientFactory>();
            var client = clientFactory.CreateClient(clientName);
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(uri),
                Method = HttpMethod.Get,
            };
            RequestHeaderAdd(request, customHeaders);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            if (!string.IsNullOrEmpty(accessToken))
                client.SetBearerToken(accessToken);
            return await GetResponseResult<T>(client, request);
        }

        public async Task<T> PostRequestAsync<T>(string clientName, string uri, Object obj, string accessToken = null, Dictionary<string, string> customHeaders = null,
                                                    string mediaType = "application/json", Encoding encoder = null)
        {
            var clientFactory = OdinInjectCore.GetService<IHttpClientFactory>();
            var client = clientFactory.CreateClient(clientName);
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(uri),
                Method = HttpMethod.Post,
            };
            RequestHeaderAdd(request, customHeaders);
            if (!string.IsNullOrEmpty(accessToken))
                client.SetBearerToken(accessToken);
            request.Content = GenerateContent(obj, mediaType, encoder);
            return await GetResponseResult<T>(client, request);
        }




        private HttpContent GenerateContent(Object obj, string mediaType, Encoding encoder)
        {
            if (typeof(String) == obj.GetType())
            {
                return GenerateContent<String>(obj.ToString(), mediaType, encoder);
            }
            else
            {
                return GenerateContent<Object>(obj, mediaType, encoder);
            }
        }
        private HttpContent GenerateContent<T>(T obj, string mediaType, Encoding encoder)
        {
            StringBuilder jsonContent = new StringBuilder();
            string sendContent = string.Empty;
            if (mediaType == "application/json")
            {
                sendContent = JsonConvert.SerializeObject(obj);
            }
            else
            {
                Dictionary<string, string> dic = ConvertPostDataToDictionary<T>(obj, encoder);
                sendContent = ConvertDictionaryToPostFormData(dic).ToString();
            }
            return new StringContent(
                            sendContent,
                            encoder == null ? Encoding.UTF8 : encoder,
                            mediaType);
        }
        private async Task<T> PostResponseResult<T>(HttpClient client, HttpRequestMessage request)
        {
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return GetResult<T>(response);
            }
            else
                throw new Exception("请求出错");
        }
        private async Task<T> GetResponseResult<T>(HttpClient client, HttpRequestMessage request)
        {
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return GetResult<T>(response);
            }
            else
                throw new Exception("请求出错");
        }
        private void RequestHeaderAdd(HttpRequestMessage request, Dictionary<string, string> customHeaders)
        {
            if (customHeaders != null)
            {
                foreach (KeyValuePair<string, string> customHeader in customHeaders)
                {
                    request.Headers.Add(customHeader.Key, customHeader.Value);
                }
            }
        }
        private T GetResult<T>(HttpResponseMessage httpResponseMessage)
        {
            // 确认响应成功，否则抛出异常
            // result.EnsureSuccessStatusCode();
            if (typeof(T) == typeof(byte[]))
            {
                return (T)Convert.ChangeType(httpResponseMessage.Content.ReadAsByteArrayAsync(), typeof(T));
            }
            else if (typeof(T) == typeof(Stream))
            {
                return (T)Convert.ChangeType(httpResponseMessage.Content.ReadAsStreamAsync().Result, typeof(T));
            }
            else
            {
                if (typeof(T) == typeof(string))
                    return (T)Convert.ChangeType(httpResponseMessage.Content.ReadAsStringAsync().Result, typeof(T));
                return JsonConvert.DeserializeObject<T>(httpResponseMessage.Content.ReadAsStringAsync().Result);
            }
        }

        public static Dictionary<string, string> ConvertPostDataToDictionary<T>(T obj, Encoding encoder = null)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (typeof(T) == typeof(String))
            {
                foreach (var item in obj.ToString().Split('&'))
                {
                    dic.Add(
                        item.Split('=')[0],
                        encoder == null || encoder == Encoding.UTF8 ?
                        item.Split('=')[1]
                        :
                        item.Split('=')[1].ConvertStringEncode(Encoding.UTF8, encoder)
                        );
                }
            }
            else
            {
                foreach (var item in obj.GetType().GetRuntimeProperties())
                {
                    dic.Add(item.Name,
                            encoder == null || encoder == Encoding.UTF8 ?
                            item.GetValue(obj).ToString()
                            :
                            item.GetValue(obj).ToString().ConvertStringEncode(Encoding.UTF8, encoder)
                            );
                }
            }
            return dic;
        }
        private StringBuilder ConvertDictionaryToPostFormData(Dictionary<string, string> dic)
        {
            StringBuilder builder = new StringBuilder();
            if (dic != null)
            {
                bool hasParam = false;
                foreach (KeyValuePair<string, string> kv in dic)
                {
                    string name = kv.Key;
                    string value = kv.Value;
                    // 忽略参数名或参数值为空的参数
                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                    {
                        if (hasParam)
                        {
                            builder.Append("&");
                        }
                        builder.Append(name);
                        builder.Append("=");
                        builder.Append(value);
                        hasParam = true;
                    }
                }
            }
            return builder;
        }
    }
}