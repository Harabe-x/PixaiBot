using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management
{
    public class TempMailApiManager : ITempMailApiManager
    {
        public event EventHandler<string> RequestFailed;

        private readonly HttpClient _httpClient;

        private List<string> _domainList;

        public TempMailApiManager()
        {
            _httpClient = new HttpClient();
        }
        public string GetEmail(string apiKey)
        {
            const string letters = "abcdefghijklmnopqrstuvwxyz";

            var random = new Random();
            
             GetDomainList(apiKey);

             if(_domainList == null) return string.Empty;

             var firstEmailPart =
                 new string(Enumerable.Repeat(letters, 6).Select(x => x[random.Next(x.Length)]).ToArray());

             return firstEmailPart + _domainList[random.Next(_domainList.Count)];
        }

        public string GetVerificationLink(string email, string apiKey)
        {
            return string.Empty;
        }

        private string HashEmail(string email)
        {
            using var md5 = MD5.Create();

            var inputBytes = Encoding.UTF8.GetBytes(email);

            var hashedBytes = md5.ComputeHash(inputBytes);

            var stringBuilder = new StringBuilder();
            foreach (var hashedByte in hashedBytes)
            {
                stringBuilder.Append(hashedByte.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        private void GetDomainList(string apiKey)
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://privatix-temp-mail-v1.p.rapidapi.com/request/domains/"),
                Headers =
                {
                    { "X-RapidAPI-Key",apiKey},
                    { "X-RapidAPI-Host", "privatix-temp-mail-v1.p.rapidapi.com" }
                }
            };
            using var response = _httpClient.Send(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                RequestFailed?.Invoke(this,response.ReasonPhrase);
                return;
            }
           
            var responseText = response.Content.ReadAsStringAsync().Result;
            _domainList = JsonSerializer.Deserialize<List<string>>(responseText);

        }

    }
}

//c9ee0d2cc8msh6d2c61055ce2ef5p142126jsn75df83703d33