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
    public string ConfigFilePath { get; }

    private readonly JsonReader _jsonReader;


    public ConfigManager()
    {
        ConfigFilePath = InitialConfiguration.UserConfigPath;
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