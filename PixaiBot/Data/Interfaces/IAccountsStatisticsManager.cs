using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces
{
    public interface IAccountsStatisticsManager
    {
        public int AccountsNumber { get; }

        public int AccountsWithUnclaimedCredits { get; }

        public int AccountsWithClaimedCredits { get; }

        public void RefreshStatistics();

        public void IncrementAccountsNumber(int number);

        public void IncrementAccountsWithClaimedCreditsNumber(int number);

        public void IncrementAccountsWithUnclaimedCreditsNumber(int number);

        public void WriteStatisticsToFile();

    }
}
