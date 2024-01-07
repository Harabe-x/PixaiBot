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
    #region Methods
    /// <summary>
    /// Saves config.
    /// </summary>
    /// <param name="config">Config to save.</param>
    public void SaveConfig(UserConfig config)
    {
        JsonWriter.WriteJson(config, InitialConfiguration.UserConfigPath);
    }
    /// <summary>
    /// Reads config
    /// </summary>
    /// <returns>User configuration.</returns>
    public UserConfig GetConfig()
    {
        var readConfig = JsonReader.ReadConfigFile(InitialConfiguration.UserConfigPath);

        if (readConfig == null)
        {
            readConfig = new UserConfig();
            SaveConfig(readConfig);
        }

        return readConfig;
    }

    #endregion
}