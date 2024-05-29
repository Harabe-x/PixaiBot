using System;
using PixaiBot.UI.Models;

namespace PixaiBot.Data.Interfaces;

public interface IBotStatisticsManager
{
    public void SaveStatistics(BotStatistics botStatistics);

    /// <summary>
    ///     Read bot statistics from file.
    /// </summary>
    /// <returns>The <see cref="BotStatistics" /></returns>
    public BotStatistics GetStatistics();

    /// <summary>
    ///     Occurs when statistics are changed.
    /// </summary>
    public event EventHandler StatisticsChanged;
}