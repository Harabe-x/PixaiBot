using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic;

public class ConfigManager : IConfigManager
{
    public string ConfigFilePath { get; }

    private readonly JsonReader _jsonReader;

    private readonly ILogger _logger;

    public ConfigManager(ILogger logger)
    {
        _logger = logger;
        ConfigFilePath = InitialConfiguration.UserConfigPath;
        _jsonReader = new JsonReader();
    }

    public UserConfig GetConfig()
    {
        _logger.Log("Readed Config File",_logger.ApplicationLogFilePath);
        return _jsonReader.ReadConfigFile(ConfigFilePath);
    }

    public void SaveConfig(UserConfig config)
    {
        _logger.Log("Writed Config File",_logger.ApplicationLogFilePath);
        JsonWriter.WriteJson(config, ConfigFilePath);
    }
}