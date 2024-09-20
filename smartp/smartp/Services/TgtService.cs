using System;
using System.Net.Http;
using System.Text;

namespace smartp.Services
{
    public class TgtService
    {
        private readonly HttpClient _client;

        public TgtService()
        {
            _client = new HttpClient();
        }

        public string GetTgt(string username, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://giris.epias.com.tr/cas/v1/tickets");
            request.Headers.Add("Accept", "text/plain");

            var content = new StringContent($"username={username}&password={password}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");

            request.Content = content;
            var response = _client.Send(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                return responseContent;
            }
            else
            {
                throw new Exception("TGT alma başarısız: " + (int)response.StatusCode);
            }
        }
    }
}
