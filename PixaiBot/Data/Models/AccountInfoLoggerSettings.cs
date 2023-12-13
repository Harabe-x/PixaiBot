using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Models
{
    internal class AccountInfoLoggerSettings
    {
        public bool ShouldLogEmailVerificationStatus { get; set; }

        public bool ShouldLogFollowingCount { get; set; }

        public bool ShouldLogFollowersCount { get; set; }

        public bool ShouldLogAccountId { get; set; }

        public bool ShouldLogAccountUsername { get; set; }


        public AccountInfoLoggerSettings()
        {
            ShouldLogAccountId = true;
            ShouldLogAccountUsername = true;
            ShouldLogEmailVerificationStatus = true;
            ShouldLogFollowersCount = true;
            ShouldLogFollowingCount = true;
        }
    }
}
