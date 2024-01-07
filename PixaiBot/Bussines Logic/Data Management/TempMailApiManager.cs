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

namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management;

public class TempMailApiManager : ITempMailApiManager
{
    #region Constructor

    public TempMailApiManager(ILogger logger)
    { 
        _httpClient = new HttpClient();
        _logger = logger;
    }

    #endregion

    #region Methods
    

    public IEnumerable<string> GetDomains(string tempMailApiKey)
    {
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
            _logger.Log("Invalid api key", _logger.CreditClaimerLogFilePath);

            RequestFailed?.Invoke(this, response.ReasonPhrase);
            return null;
        }

        var responseText = response.Content.ReadAsStringAsync().Result;
        return _domainList = JsonSerializer.Deserialize<List<string>>(responseText);
    }

    public string GetVerificationLink(string email, string apiKey)
    {
        _logger.Log("Getting verification link", _logger.CreditClaimerLogFilePath);
        var hashedEmail = HashEmail(email);

        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://privatix-temp-mail-v1.p.rapidapi.com/request/mail/id/{hashedEmail}/"),
            Headers =
            {
                { "X-RapidAPI-Key", apiKey },
                { "X-RapidAPI-Host", "privatix-temp-mail-v1.p.rapidapi.com" }
            }
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
        foreach (var hashedByte in hashedBytes) stringBuilder.Append(hashedByte.ToString("x2"));

        _logger.Log("Hashed email to request", _logger.CreditClaimerLogFilePath);
        return stringBuilder.ToString();
    }

    private string GetUrlFromString(string url)
    {
        const string extractUrlRegexPattern =
            @"(http|ftp|https):\/\/([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:\/~+#-]*[\w@?^=%&\/~+#-])";
        var extractedUrl = Regex.Match(url, extractUrlRegexPattern);
        _logger.Log("Extracted url from email", _logger.CreditClaimerLogFilePath);
        return extractedUrl.Value;
    }

    #endregion

    #region Fields

    public event EventHandler<string> RequestFailed;

    private readonly HttpClient _httpClient;

    private IList<string> _domainList;

    private readonly ILogger _logger;

    #endregion
}