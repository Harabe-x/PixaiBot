using PixaiBot.Data.Interfaces;
using System.IO;
using System;
using System.Text;

namespace PixaiBot.Bussines_Logic;

public class Logger : ILogger
{
    public string CreditClaimerLogFilePath { get; }

    public string ApplicationLogFilePath { get; }

    private StringBuilder _builder;

    private string lastFilePath;

    private bool _previousWasError;

    public Logger()
    {
        CreditClaimerLogFilePath =
            $@"{InitialConfiguration.BotLogsPath}\CreditClaimer Log {DateTime.Now:yyyy-MM-dd}.txt";
        ApplicationLogFilePath = $@"{InitialConfiguration.BotLogsPath}\Application Log {DateTime.Now:yyyy-MM-dd}.txt";
        if (!File.Exists(CreditClaimerLogFilePath)) File.Create(CreditClaimerLogFilePath);
        if (!File.Exists(ApplicationLogFilePath)) File.Create(ApplicationLogFilePath);
        _builder = new StringBuilder();
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

            if (!_previousWasError || filePath != lastFilePath || string.IsNullOrEmpty(lastFilePath)) return;

            File.AppendAllText(lastFilePath, $"[{DateTime.Now:HH:mm:ss}] {_builder}\n");
            _previousWasError = false;
        }
        catch (Exception e)
        {
            lastFilePath = filePath;
            _builder.AppendLine(message);
            _previousWasError = true;
        }
    }
}