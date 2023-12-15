using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic;


//TODO: Possible refactor  

public class ConfigManager : IConfigManager
{
    public void SaveConfig(UserConfig config)
    {
        JsonWriter.WriteJson(config, InitialConfiguration.UserConfigPath);
    }

    public UserConfig GetConfig()
    {
        var readConfig = JsonReader.ReadConfigFile(InitialConfiguration.UserConfigPath);

        if (readConfig != null) return readConfig;
        
        SaveConfig(readConfig);
        
        return readConfig ;

    }
}