using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic
{
    internal class ConfigManager : IConfigManager
    {

        public const string ConfigFilePath = "config.json";

        private readonly JsonReader _jsonReader;

        private readonly JsonWriter _jsonWriter;

        public ConfigManager()
        {
            _jsonReader = new JsonReader();
            _jsonWriter = new JsonWriter();
        }
         
        public UserConfig GetConfig()
        {
            return _jsonReader.ReadConfigFile(ConfigFilePath);
        }

        public void SaveConfig(UserConfig config)
        {
            _jsonWriter.WriteJson(config, ConfigFilePath);
        }
    }
}
