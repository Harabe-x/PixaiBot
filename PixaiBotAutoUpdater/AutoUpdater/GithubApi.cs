using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PixaiBot.AutoUpdater;

namespace PixaiBotAutoUpdater.AutoUpdater
{
    internal class GithubApi
    {

        public GithubApi()
        {
            _httpClient = new HttpClient();
        }

        public string GetLatestVersion()
        {
            return  string.IsNullOrEmpty(LatestRelease.TagName) ? string.Empty : LatestRelease.TagName[1..];    
        }
        public string GetLatestReleaseDownloadUrl()
        {
            return LatestRelease.Assets?.FirstOrDefault(x => x.DownloadUrl.Contains("bin"))?.DownloadUrl ?? string.Empty;
        }

        private async Task<GithubRelease> GetLatestRelease()
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,  
                RequestUri = new Uri("https://api.github.com/repos/Harabe-x/PixaiBot/releases/latest"),
                Headers =
                {
                    { "User-Agent" , "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36"}
                }
            };

            var response = await _httpClient.SendAsync(requestMessage);
            
            var responseText = await response.Content.ReadAsStringAsync();


            return JsonConvert.DeserializeObject<GithubRelease>(responseText);
        }

        private GithubRelease? _latestRelease;

        private GithubRelease? LatestRelease
        {
            get => _latestRelease ??= GetLatestRelease().Result;
            set => _latestRelease = value;
        }

        private  readonly HttpClient _httpClient;
    }
}
