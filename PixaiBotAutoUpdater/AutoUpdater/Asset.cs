using Newtonsoft.Json;

namespace PixaiBotAutoUpdater.AutoUpdater;

internal class Asset
{
    [JsonProperty("browser_download_url")] public string DownloadUrl { get; set; }
}