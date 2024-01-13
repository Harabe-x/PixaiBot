using System;
using PixaiBot.Business_Logic.Data_Handling;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Models;

namespace PixaiBot.Business_Logic.Data_Management;

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