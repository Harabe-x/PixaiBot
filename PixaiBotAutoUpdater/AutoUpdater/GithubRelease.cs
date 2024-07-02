using Newtonsoft.Json;

namespace PixaiBotAutoUpdater.AutoUpdater;

internal class GithubRelease
{
    [JsonProperty("tag_name")] public string TagName { get; set; }

    [JsonProperty("assets")] public IEnumerable<Asset>? Assets { get; set; }
}