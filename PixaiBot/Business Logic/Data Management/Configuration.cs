using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using PixaiBot.Business_Logic.Data_Handling;
using PixaiBot.UI.Models;

namespace PixaiBot.Business_Logic.Data_Management;

//TODO: Refactor      
public static class Configuration
{
    #region Constructor

    static Configuration()
    {
        IsDevEnv = false;


        ApplicationDataPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PixaiAutoClaimer");
        UserConfigPath =
            Path.Combine(ApplicationDataPath, "config.json");
        BotLogsPath =
            Path.Combine(ApplicationDataPath, "Logs");
        StatisticsFilePath =
            Path.Combine(ApplicationDataPath, "statistics.json");
        AccountsFilePath =
            Path.Combine(ApplicationDataPath, "accounts.json");
        BotVersion =
            Assembly.GetExecutingAssembly().GetName().Version.ToString();
        ApiKeysFilePath =
            Path.Combine(ApplicationDataPath, "apiKeys.json");
        CreditClaimerLogFilePath =
            Path.Combine(BotLogsPath, $"CreditClaimer Log {DateTime.Now:yyyy-MM-dd}.txt");
        ApplicationLogFilePath =
            Path.Combine(BotLogsPath, $"Application Log {DateTime.Now:yyyy-MM-dd}.txt");


        if (!Directory.Exists(ApplicationDataPath)) CreateDirectories();
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Creates the directories needed for the bot to work
    /// </summary>
    private static void CreateDirectories()
    {
        Directory.CreateDirectory(
            $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\PixaiAutoClaimer\Logs");
    }


    /// <summary>
    ///     Creates the config file if it doesn't exist
    /// </summary>
    public static void CreateConfigFile()
    {
        if (File.Exists(UserConfigPath)) return;

        var userConfig = new UserConfig
        {
            StartWithSystem = false,
            ToastNotifications = false,
            CreditsAutoClaim = false,
            MultiThreading = false,
            HeadlessBrowser = false,
            NumberOfThreads = 1
        };
        JsonWriter.WriteJson(userConfig, UserConfigPath);
    }


    /// <summary>
    ///     Creates the statistics file if it doesn't exist
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
        }

        ;

        var statistics = new BotStatistics
        {
            AccountsCount = 0,
            BotVersion = BotVersion,
            LastCreditClaimDateTime = DateTime.Now
        };
        JsonWriter.WriteJson(statistics, StatisticsFilePath);
    }

    public static void CreateApiKeysFile()
    {
        if (File.Exists(ApiKeysFilePath)) return;

        var apiKeys = new Dictionary<string, IEnumerable<string>>();
        JsonWriter.WriteJson(apiKeys, ApiKeysFilePath);
    }

    #endregion

    #region Fields

    /// <summary>
    ///     Path to Credit Claimer Logs
    /// </summary>
    public static string CreditClaimerLogFilePath { get; }


    /// <summary>
    ///     Path to Credit Claimer Logs
    /// </summary>
    public static string ApplicationLogFilePath { get; }

    /// <summary>
    ///     Current Bot Version
    /// </summary>
    public static string BotVersion { get; }


    /// <summary>
    ///     Path to the user config file
    /// </summary>
    public static string UserConfigPath { get; }


    /// <summary>
    ///     Path to the bot logs folder
    /// </summary>
    public static string BotLogsPath { get; }


    /// <summary>
    ///     Path to the bot statistics file
    /// </summary>
    public static string StatisticsFilePath { get; }

    /// <summary>
    ///     Path to the accounts file
    /// </summary>
    public static string AccountsFilePath { get; }


    /// <summary>
    ///     Temp-mail domain list
    /// </summary>
    public static string ApiKeysFilePath { get; }


    /// <summary>
    ///     Path to the application data folder
    /// </summary>
    public static string ApplicationDataPath { get; }

    
    public static bool IsDevEnv { get; }

    #endregion
}