using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using PixaiBot.UI.Models;

namespace PixaiBot.Business_Logic.Data_Handling;

public static class JsonReader
{
    /// <summary>
    ///     Reads a json file and returns a list of user accounts.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static IList<UserAccount>? ReadAccountFile(string filePath)
    {
        var jsonString = File.ReadAllText(filePath);
        var accounts = JsonSerializer.Deserialize<IList<UserAccount>>(jsonString);
        return accounts;
    }


    /// <summary>
    ///     Reads a json file and returns a user config object.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static UserConfig? ReadConfigFile(string filePath)
    {
        var jsonString = File.ReadAllText(filePath);
        var userConfig = JsonSerializer.Deserialize<UserConfig>(jsonString);
        return userConfig;
    }


    /// <summary>
    ///     Reads a json file and returns a bot statistics object.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static BotStatistics? ReadStatisticsFile(string filePath)
    {
        var jsonString = File.ReadAllText(filePath);
        var statistics = JsonSerializer.Deserialize<BotStatistics>(jsonString);
        return statistics;
    }

    public static Dictionary<string, IEnumerable<string>>? GetDomainsAssociatedWithApiKeys(string filePath)
    {
        var jsonString = File.ReadAllText(filePath);
        var domains = JsonSerializer.Deserialize<Dictionary<string, IEnumerable<string>>>(jsonString);
        return domains;
    }
}