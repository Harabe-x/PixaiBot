using System;
using System.IO;
using System.Reflection;
using PixaiBot.Business_Logic.Data_Handling;
using PixaiBot.UI.Models;

namespace PixaiBot.Business_Logic.Data_Management;

//TODO: Refactor      
public static class InitialConfiguration
{
    #region Constructor

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

    #endregion

    #region Methods

    /// <summary>
    /// Creates the directories needed for the bot to work
    /// </summary>
    public static void CreateDirectories()
    {
        Directory.CreateDirectory(
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\PixaiAutoClaimer\\Logs");
    }


    /// <summary>
    /// Creates the config file if it doesn't exist
    /// </summary>
    public static void CreateConfigFile()
    {
     

        var userConfig = new UserConfig()
        {
            StartWithSystem = false,
            ToastNotifications = false,
            CreditsAutoClaim = false,
            MultiThreading = false,
            HeadlessBrowser = false,
            NumberOfThreads = 1,
        };
        JsonWriter.WriteJson(userConfig, UserConfigPath);
    }


    /// <summary>
    /// Creates the statistics file if it doesn't exist
    /// </summary>
    public static void CreateStatisticsFile()
    {
        if (File.Exists(StatisticsFilePath))
        {
            var currentStatistics = JsonReader.ReadStatisticsFile(StatisticsFilePath);
            if (currentStatistics.BotVersion == BotVersion) return;
            currentStatistics.BotVersion = BotVersion;
            JsonWriter.WriteJson(currentStatistics, StatisticsFilePath);
            return;
        };

        var statistics = new BotStatistics()
        {
            AccountsCount = 0,
            BotVersion = BotVersion,
            LastCreditClaimDateTime = DateTime.Now
        };
        JsonWriter.WriteJson(statistics, StatisticsFilePath);
    }

    #endregion

    #region Fields

    /// <summary>
    /// Current Bot Version
    /// </summary>
    public static string BotVersion { get; }


    /// <summary>
    /// Path to the user config file
    /// </summary>
    public static string UserConfigPath { get; }


    /// <summary>
    /// Path to the bot logs folder
    /// </summary>
    public static string BotLogsPath { get; }


    /// <summary>
    /// Path to the bot statistics file
    /// </summary>
    public static string StatisticsFilePath { get; }

    /// <summary>
    /// Path to the accounts file 
    /// </summary>
    public static string AccountsFilePath { get; }


    /// <summary>
    /// Path to the application data folder
    /// </summary>
    public static string ApplicationDataPath { get; }

    #endregion
}