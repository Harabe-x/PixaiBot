using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic
{
    public class JsonReader
    {
        public IList<UserAccount> ReadAccountFile(string filePath) 
        {
            var jsonString = File.ReadAllText(filePath);
            var accounts = JsonSerializer.Deserialize<IList<UserAccount>>(jsonString);
            return accounts;
        }

        public UserConfig ReadConfigFile(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);
            var userConfig = JsonSerializer.Deserialize<UserConfig>(jsonString);

            return userConfig; 
        }

        public AccountsStatistics ReadStatisticsFile(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);
            var statistics = JsonSerializer.Deserialize<AccountsStatistics>(jsonString);
            return statistics;
        }

    }
}
