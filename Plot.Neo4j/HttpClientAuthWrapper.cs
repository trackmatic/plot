using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient.Execution;

namespace Plot.Neo4j
{
    public class HttpClientAuthWrapper : IHttpClient
    {
        private readonly AuthenticationHeaderValue _authenticationHeader;
        private readonly HttpClient _client;

        public HttpClientAuthWrapper(string username, string password)
        {
            _client = new HttpClient();
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return;
            }
            var encoded = Encoding.ASCII.GetBytes($"{username}:{password}");
            _authenticationHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(encoded));
            Username = username;
            Password = password;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            if (_authenticationHeader != null)
                request.Headers.Authorization = _authenticationHeader;
            return _client.SendAsync(request);
        }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}