using System;
using PixaiBot.Business_Logic.Data_Handling;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Models;

namespace PixaiBot.Business_Logic.Data_Management;

public class BotStatisticsManager : IBotStatisticsManager
{
    #region Constructor

    public BotStatisticsManager(ILogger logger)
    {
        _logger = logger;
    }

    #endregion

    #region Methods

    public void SaveStatistics(BotStatistics botStatistics)
    {
        _logger.Log("Saving statistics file", _logger.ApplicationLogFilePath);
        JsonWriter.WriteJson(botStatistics, InitialConfiguration.StatisticsFilePath);
        StatisticsChanged?.Invoke(this, EventArgs.Empty);
    }

    public BotStatistics GetStatistics()
    {
        _logger.Log("Reading statistics file", _logger.ApplicationLogFilePath);
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

    private readonly ILogger _logger;

    #endregion
}