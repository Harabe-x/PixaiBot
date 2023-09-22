using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.DevTools;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic
{
    internal static class InitialConfiguration
    {
        public static string UserConfigPath { get; }

        public static string BotLogsPath { get; }

        public static string StatisticsFilePath { get; }

        public static string AccountsFilePath { get; }

        public static string ApplicationDataPath { get; }

        static InitialConfiguration()
        {
            ApplicationDataPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\PixaiAutoClaimer";
            UserConfigPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\PixaiAutoClaimer\\config.json";
            BotLogsPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\PixaiAutoClaimer\\Logs";
            StatisticsFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\PixaiAutoClaimer\\statistics.json";
            AccountsFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\PixaiAutoClaimer\\accounts.json";

            if (!Directory.Exists(ApplicationDataPath))
            {
                CreateDirectories();
            }
        }

        public static void CreateDirectories()
        {
            Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\PixaiAutoClaimer\\Logs");
        }



        public static void CreateConfigFile()
        {


            if (!File.Exists(UserConfigPath))
            {
                var userConfig = new UserConfig()
                {
                    StartWithSystem = false,
                    ToastNotifications = false,
                    CreditsAutoClaim = false
                };
                JsonWriter.WriteJson(userConfig, UserConfigPath);
            }
        }

        public static void CreateStatisticsFile()
        {
            if (!File.Exists(StatisticsFilePath))
            {
                var statistics = new AccountsStatistics()
                {
                    AccountsCount = 0,
                    AccountWithUnclaimedCredits = 0,
                    AccountWithClaimedCredits = 0
                };
                JsonWriter.WriteJson(statistics, StatisticsFilePath);
            };
        }

    }

}
