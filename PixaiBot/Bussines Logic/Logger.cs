using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Bussines_Logic;

internal class Logger : ILogger
{
    public string CreditClaimerLogFilePath { get; }

    public string ApplicationLogFilePath { get; }

    public Logger()
    {
        CreditClaimerLogFilePath = $@"{InitialConfiguration.BotLogsPath}\CreditClaimer Log {DateTime.Now:yyyy-MM-dd}.txt";
        ApplicationLogFilePath = $@"{InitialConfiguration.BotLogsPath}\Application Log {DateTime.Now:yyyy-MM-dd}.txt";
        
        if (!File.Exists(CreditClaimerLogFilePath)) File.Create(CreditClaimerLogFilePath); ;
        if (!File.Exists(ApplicationLogFilePath)) File.Create(ApplicationLogFilePath); ;
    }

    public void Log(string message, string filePath)
    {
        File.AppendAllText(filePath, $"[{DateTime.Now}] {message}\n");
    }
}