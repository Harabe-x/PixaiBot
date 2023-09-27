using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces;

public interface IBotStatisticsManager
{
    public int AccountsNumber { get; }

    public DateTime LastCreditClaimDateTime { get; }

    public string BotVersion { get; }

    public void RefreshStatistics();

    public void IncreaseAccountsCount(int number);

    public void SetClaimDateTime(DateTime creditClaimDate);

    public void ResetNumberOfAccounts();

    public void SetApplicationVersion();

    public void SaveStatistics();
}