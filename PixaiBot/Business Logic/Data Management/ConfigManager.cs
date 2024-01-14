using PixaiBot.Business_Logic.Data_Handling;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Models;

namespace PixaiBot.Business_Logic.Data_Management;

public class ConfigManager : IConfigManager
{
    #region Constructor

    public ConfigManager(ILogger logger)
    {
        _logger = logger;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Saves config.
    /// </summary>
    /// <param name="config">Config to save.</param>
    public void SaveConfig(UserConfig config)
    {
        _logger.Log("Saving config file", _logger.ApplicationLogFilePath);
        JsonWriter.WriteJson(config, InitialConfiguration.UserConfigPath);
    }

    /// <summary>
    /// Reads config
    /// </summary>
    /// <returns>User configuration.</returns>
    public UserConfig GetConfig()
    {
        _logger.Log("Reading config file", _logger.ApplicationLogFilePath);
        var readConfig = JsonReader.ReadConfigFile(InitialConfiguration.UserConfigPath);

        if (readConfig == null)
        {
            readConfig = new UserConfig();
            SaveConfig(readConfig);
        }

        return readConfig;
    }

    #endregion

    #region Fields

    private readonly ILogger _logger;

    #endregion
}