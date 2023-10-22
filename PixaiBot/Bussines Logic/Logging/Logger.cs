using PixaiBot.Data.Interfaces;
using System.IO;
using System;

namespace PixaiBot.Bussines_Logic;

public class Logger : ILogger
{
    public string CreditClaimerLogFilePath { get; }

    public string ApplicationLogFilePath { get; }

    public Logger()
    {
        CreditClaimerLogFilePath =
            $@"{InitialConfiguration.BotLogsPath}\CreditClaimer Log {DateTime.Now:yyyy-MM-dd}.txt";
        ApplicationLogFilePath = $@"{InitialConfiguration.BotLogsPath}\Application Log {DateTime.Now:yyyy-MM-dd}.txt";

        if (!File.Exists(CreditClaimerLogFilePath)) File.Create(CreditClaimerLogFilePath);
        ;
        if (!File.Exists(ApplicationLogFilePath)) File.Create(ApplicationLogFilePath);
        ;
    }

    /// <summary>
    /// Logs a message to the log file  
    /// </summary>
    /// <param name="message"></param>
    /// <param name="filePath"></param>
    public void Log(string message, string filePath)
    {
        File.AppendAllText(filePath, $"[{DateTime.Now:HH:mm:ss}] {message}\n");
    }
}