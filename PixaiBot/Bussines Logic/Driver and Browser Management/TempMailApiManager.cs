using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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
            if (_domainList == null)
            {
                GetDomainList(apiKey);
            }

            // If the api key is invalid, the domain list will be null, so check if it is null and return an empty string 
            if (_domainList == null) return string.Empty;

            // Generate string with 6 random letters

            var firstEmailPart = new string(Enumerable.Repeat(letters, 6).Select(x => x[random.Next(x.Length)]).ToArray());

            return firstEmailPart + _domainList[random.Next(_domainList.Count)];
        }

        public string GetVerificationLink(string email, string apiKey)
        {
            var hashedEmail = HashEmail(email);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://privatix-temp-mail-v1.p.rapidapi.com/request/mail/id/{hashedEmail}/"),
                Headers =
                {
                    { "X-RapidAPI-Key", apiKey },
                    { "X-RapidAPI-Host", "privatix-temp-mail-v1.p.rapidapi.com" },
                },
            };

            var response = _httpClient.Send(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                RequestFailed?.Invoke(this, "An error occurred,");
                return string.Empty;
            }

            var responseText = response.Content.ReadAsStringAsync().Result;

            return GetUrlFromString(responseText);
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
                    { "X-RapidAPI-Key", apiKey },
                    { "X-RapidAPI-Host", "privatix-temp-mail-v1.p.rapidapi.com" }
                }
            };

            var response = _httpClient.Send(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                RequestFailed?.Invoke(this, response.ReasonPhrase);
                return;
            }

            var responseText = response.Content.ReadAsStringAsync().Result;
            _domainList = JsonSerializer.Deserialize<List<string>>(responseText);
        }

        private string GetUrlFromString(string url)
        {
            var extractUrlRegexPattern = @"(http|ftp|https):\/\/([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:\/~+#-]*[\w@?^=%&\/~+#-])";
            var extractedUrl = Regex.Match(url, extractUrlRegexPattern);
            return extractedUrl.Value;
        }
    }
}
