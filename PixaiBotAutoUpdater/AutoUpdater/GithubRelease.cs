using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PixaiBotAutoUpdater.AutoUpdater
{
    internal class GithubRelease
    {
        [JsonProperty("tag_name")]
        public string TagName { get; set; }

        [JsonProperty("assets")]
        public IEnumerable<Asset>? Assets { get; set; }

    }
}
