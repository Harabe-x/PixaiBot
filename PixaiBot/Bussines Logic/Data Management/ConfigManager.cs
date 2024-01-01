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

    public void SaveConfig(UserConfig config)
    {
        JsonWriter.WriteJson(config, InitialConfiguration.UserConfigPath);
    }

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