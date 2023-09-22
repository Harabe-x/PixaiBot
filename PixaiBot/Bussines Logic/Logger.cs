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
    public string LogFilePath { get; }

    public Logger()
    {                         
        LogFilePath = $@"{InitialConfiguration.BotLogsPath}\{DateTime.Now:yyyy-MM-dd}.txt";
        if (File.Exists(LogFilePath)) return;

        File.Create(LogFilePath);
    }

    public void Log(string message)
    {
        File.AppendAllText(LogFilePath, $"[{DateTime.Now}] {message}\n");
    }
}