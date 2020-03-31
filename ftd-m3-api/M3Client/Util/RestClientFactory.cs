using M3Service.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace M3Service.Util
{
    public class RestClientFactory
    {
        public static HttpClient CreateBasicAuthRestClient(ClientConfiguration clientConfig)
        {
            var client = new HttpClient();
            var authenticationBytes = Encoding.ASCII.GetBytes(clientConfig.User + ":" + clientConfig.Password);

            client.BaseAddress = new Uri(clientConfig.ServiceUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authenticationBytes));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(clientConfig.ContentType));
            return client;
        }
    }
}
