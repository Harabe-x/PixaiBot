using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic;

internal class ConfigManager : IConfigManager
{
    public const string ConfigFilePath = @"C:\Users\xgra5\AppData\Roaming\PixaiAutoClaimer\config.json";

    private readonly JsonReader _jsonReader;


    public ConfigManager()
    {
        _jsonReader = new JsonReader();
    }

    public UserConfig GetConfig()
    {
        return _jsonReader.ReadConfigFile(ConfigFilePath);
    }

    public void SaveConfig(UserConfig config)
    {
        JsonWriter.WriteJson(config, ConfigFilePath);
    }
}