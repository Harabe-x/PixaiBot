using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Models;

namespace PixaiBot.Data.Interfaces;

public interface IBotStatisticsManager
{
    public void SaveStatistics(BotStatistics botStatistics);

    public BotStatistics GetStatistics();

    public event EventHandler StatisticsChanged;
}