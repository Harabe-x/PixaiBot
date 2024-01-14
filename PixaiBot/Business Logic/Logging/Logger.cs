using System;
using System.IO;
using System.Text;
using PixaiBot.Business_Logic.Data_Management;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Business_Logic.Logging;

public class Logger : ILogger
{
    public string CreditClaimerLogFilePath { get; }

    public string ApplicationLogFilePath { get; }

    public Logger()
    {
        CreditClaimerLogFilePath =
            $@"{InitialConfiguration.BotLogsPath}\CreditClaimer Log {DateTime.Now:yyyy-MM-dd}.txt";
        ApplicationLogFilePath = $@"{InitialConfiguration.BotLogsPath}\Application Log {DateTime.Now:yyyy-MM-dd}.txt";

        if (!File.Exists(CreditClaimerLogFilePath)) File.Create(CreditClaimerLogFilePath).Close();
        if (!File.Exists(ApplicationLogFilePath)) File.Create(ApplicationLogFilePath).Close();
    }

    /// <summary>
    /// Logs a message to the log file  
    /// </summary>
    /// <param name="message"></param>
    /// <param name="filePath"></param>
    public void Log(string message, string filePath)
    {
        try
        {
            File.AppendAllText(filePath, $"[{DateTime.Now:HH:mm:ss}] {message}\n");
        }
        catch (IOException)
        {
        }
    }
}