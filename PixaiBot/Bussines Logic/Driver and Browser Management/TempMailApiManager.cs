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
    //TODO: Refactor

    public class TempMailApiManager : ITempMailApiManager
    {
        public event EventHandler<string> RequestFailed;

        private readonly HttpClient _httpClient;

        private List<string> _domainList;

        public TempMailApiManager(ILogger logger, ITcpServerConnector tcpServerConnector)
        {
            _httpClient = new HttpClient();
            _tcpServerConnector = tcpServerConnector;
            _logger = logger;

        }

        private readonly ITcpServerConnector _tcpServerConnector;

        private readonly ILogger _logger;

        public string GetEmail(string apiKey)
        {
            const string letters = "abcdefghijklmnopqrstuvwxyz";

            var random = new Random();
            if (_domainList == null)
            {
                GetDomains(apiKey);
            }

            // If the api key is invalid, the domain list will be null, so check if it is null and return an empty string 
            if (_domainList == null) return string.Empty;

            // Generate string with 6 random letters

            var firstEmailPart = new string(Enumerable.Repeat(letters, 6).Select(x => x[random.Next(x.Length)]).ToArray());

            return firstEmailPart + _domainList[random.Next(_domainList.Count)];
        }

        public IEnumerable<string> GetDomains(string tempMailApiKey)
        {
            _tcpServerConnector.SendMessage("cGetting domains from temp mail api");

            _logger.Log("Getting domains from temp mail api", _logger.CreditClaimerLogFilePath);
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://privatix-temp-mail-v1.p.rapidapi.com/request/domains/"),
                Headers =
                {
                    { "X-RapidAPI-Key", tempMailApiKey },
                    { "X-RapidAPI-Host", "privatix-temp-mail-v1.p.rapidapi.com" }
                }
            };

            var response = _httpClient.Send(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                _tcpServerConnector.SendMessage("rInvalid api key");
                _logger.Log("Invalid api key", _logger.CreditClaimerLogFilePath);

                RequestFailed?.Invoke(this, response.ReasonPhrase);
                return null;
            }
            _tcpServerConnector.SendMessage("gReceived domain list successfully ");

            var responseText = response.Content.ReadAsStringAsync().Result;
            return _domainList = JsonSerializer.Deserialize<List<string>>(responseText);
        }

        public string GetVerificationLink(string email, string apiKey)
        {

            _tcpServerConnector.SendMessage("cGetting Verification link");


            _logger.Log("Getting verification link", _logger.CreditClaimerLogFilePath);
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
                _tcpServerConnector.SendMessage("rGetting verification link failed ");

                RequestFailed?.Invoke(this, "An error occurred,");
                return string.Empty;
            }

            var responseText = response.Content.ReadAsStringAsync().Result;

            _tcpServerConnector.SendMessage("cReceived text from temp mail api");


            return GetUrlFromString(responseText);
        }

        private string HashEmail(string email)
        {
            _tcpServerConnector.SendMessage("cHashed email for request");


            using var md5 = MD5.Create();

            var inputBytes = Encoding.UTF8.GetBytes(email);

            var hashedBytes = md5.ComputeHash(inputBytes);

            var stringBuilder = new StringBuilder();
            foreach (var hashedByte in hashedBytes)
            {
                stringBuilder.Append(hashedByte.ToString("x2"));
            }

            _logger.Log("Hashed email to request", _logger.CreditClaimerLogFilePath);
            return stringBuilder.ToString();
        }

        private string GetUrlFromString(string url)
        {
            var extractUrlRegexPattern = @"(http|ftp|https):\/\/([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:\/~+#-]*[\w@?^=%&\/~+#-])";
            var extractedUrl = Regex.Match(url, extractUrlRegexPattern);
            _logger.Log("Extracted url from email", _logger.CreditClaimerLogFilePath);
            return extractedUrl.Value;
        }
    }
}
