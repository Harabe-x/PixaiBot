using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Models;

public class BotStatistics
{
    public int AccountsCount { get; set; }

    public DateTime LastCreditClaimDateTime { get; set; }

    public string? BotVersion { get; set; }
}