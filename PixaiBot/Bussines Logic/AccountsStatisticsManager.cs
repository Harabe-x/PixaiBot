using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic
{
    internal class AccountsStatisticsManager : IAccountsStatisticsManager
    {
        private const string AccountsStatisticsFilePath = @"C:\Users\xgra5\AppData\Roaming\PixaiAutoClaimer\accountsStatistics.json";

        private AccountsStatistics _accountsStatistics;

        private JsonReader _jsonReader;

        public AccountsStatisticsManager()
        {
            _jsonReader = new JsonReader();
            RefreshStatistics();
        }

        public int AccountsNumber { get; private set; }

        public int AccountsWithUnclaimedCredits { get; private set; }

        public int AccountsWithClaimedCredits { get; private set; }

        public void RefreshStatistics()
        {
            _accountsStatistics = _jsonReader.ReadStatisticsFile(AccountsStatisticsFilePath);
            AccountsNumber = _accountsStatistics.AccountsCount;
            AccountsWithUnclaimedCredits = _accountsStatistics.AccountWithUnclaimedCredits;
            AccountsWithClaimedCredits = _accountsStatistics.AccountWithClaimedCredits;
        }

        public void IncrementAccountsNumber(int number)
        {
            _accountsStatistics.AccountsCount += number;
            JsonWriter.WriteJson(_accountsStatistics, AccountsStatisticsFilePath);
            RefreshStatistics();

        }

        public void IncrementAccountsWithClaimedCreditsNumber(int number)
        {
            _accountsStatistics.AccountWithClaimedCredits += number;
            JsonWriter.WriteJson(_accountsStatistics, AccountsStatisticsFilePath);
            RefreshStatistics();
        }

        public void IncrementAccountsWithUnclaimedCreditsNumber(int number)
        {
            _accountsStatistics.AccountWithUnclaimedCredits += number;
            JsonWriter.WriteJson(_accountsStatistics, AccountsStatisticsFilePath);
            RefreshStatistics();
        }

        public void WriteStatisticsToFile()
        {
            JsonWriter.WriteJson(_accountsStatistics, AccountsStatisticsFilePath);
        }
    }
}
