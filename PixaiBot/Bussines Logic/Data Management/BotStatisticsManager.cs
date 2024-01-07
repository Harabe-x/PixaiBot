using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic;

public class BotStatisticsManager : IBotStatisticsManager
{
    #region Methods

    public void SaveStatistics(BotStatistics botStatistics)
    {
        JsonWriter.WriteJson(botStatistics, InitialConfiguration.StatisticsFilePath);
        StatisticsChanged?.Invoke(this, EventArgs.Empty);
    }

    public BotStatistics GetStatistics()
    {
        var botStatistics = JsonReader.ReadStatisticsFile(InitialConfiguration.StatisticsFilePath);

        if (botStatistics == null)
        {
            botStatistics = new BotStatistics();
            SaveStatistics(botStatistics);
        }

        return botStatistics;
    }

    #endregion

    #region Fields

    public event EventHandler? StatisticsChanged;

    #endregion
}