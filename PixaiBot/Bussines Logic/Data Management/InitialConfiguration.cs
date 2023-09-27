using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.DevTools;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic;

public static class InitialConfiguration
{
    public static string BotVersion { get; }

    public static string UserConfigPath { get; }

    public static string BotLogsPath { get; }

    public static string StatisticsFilePath { get; }

    public static string AccountsFilePath { get; }

    public static string ApplicationDataPath { get; }

    static InitialConfiguration()
    {
        ApplicationDataPath =
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\PixaiAutoClaimer";
        UserConfigPath =
            $"{ApplicationDataPath}\\config.json";
        BotLogsPath = $"{ApplicationDataPath}\\Logs";
        StatisticsFilePath =
            $"{ApplicationDataPath}\\statistics.json";
        AccountsFilePath =
            $"{ApplicationDataPath}\\accounts.json";
        BotVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        if (!Directory.Exists(ApplicationDataPath)) CreateDirectories();
    }

    public static void CreateDirectories()
    {
        Directory.CreateDirectory(
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\PixaiAutoClaimer\\Logs");
    }


    public static void CreateConfigFile()
    {
        if (File.Exists(UserConfigPath)) return;

        var userConfig = new UserConfig()
        {
            StartWithSystem = false,
            ToastNotifications = false,
            CreditsAutoClaim = false
        };
        JsonWriter.WriteJson(userConfig, UserConfigPath);
    }

    public static void CreateStatisticsFile()
    {
        if (File.Exists(StatisticsFilePath)) return;

        var statistics = new BotStatistics()
        {
            AccountsCount = 0,
            BotVersion = BotVersion,
            LastCreditClaimDateTime = DateTime.Now
        };
        JsonWriter.WriteJson(statistics, StatisticsFilePath);
    }
}