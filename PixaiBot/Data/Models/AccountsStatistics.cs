using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Models
{
    public class AccountsStatistics
    {
        public int AccountsCount { get; set; }

        public int AccountWithClaimedCredits { get; set; }

        public int AccountWithUnclaimedCredits { get; set; }
    }
}
