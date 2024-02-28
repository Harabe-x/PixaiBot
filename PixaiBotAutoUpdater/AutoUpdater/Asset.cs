using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PixaiBotAutoUpdater.AutoUpdater
{
    internal class Asset
    {
        [JsonProperty("browser_download_url")]
        public string DownloadUrl { get; set; }
    }
}
