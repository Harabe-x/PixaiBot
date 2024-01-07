using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces;

public interface ILogger
{

    /// <summary>
    ///  The path to the log file for the credit claimer.
    /// </summary>
    public string CreditClaimerLogFilePath { get; }
    /// <summary>
    ///   The path to the log file for the application.
    /// </summary>
    public string ApplicationLogFilePath { get; }


    /// <summary>
    ///  Logs a message to the file indicated in <paramref name="filePath"/>.
    /// </summary>
    /// <param name="message">Message to log.</param>
    /// <param name="filePath">Log file path.</param>
    public void Log(string message, string filePath);
}