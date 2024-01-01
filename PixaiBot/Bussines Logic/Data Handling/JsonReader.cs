using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic;

public static class JsonReader
{
    /// <summary>
    /// Reads a json file and returns a list of user accounts
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static IList<UserAccount> ReadAccountFile(string filePath)
    {
        var jsonString = File.ReadAllText(filePath);
        var accounts = JsonSerializer.Deserialize<IList<UserAccount>>(jsonString);
        return accounts;
    }


    /// <summary>
    /// Reads a json file and returns a user config object
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static UserConfig ReadConfigFile(string filePath)
    {
        var jsonString = File.ReadAllText(filePath);
        var userConfig = JsonSerializer.Deserialize<UserConfig>(jsonString);

        return userConfig;
    }


    /// <summary>
    /// Reads a json file and returns a bot statistics object
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static BotStatistics ReadStatisticsFile(string filePath)
    {
        var jsonString = File.ReadAllText(filePath);
        var statistics = JsonSerializer.Deserialize<BotStatistics>(jsonString);
        return statistics;
    }
}