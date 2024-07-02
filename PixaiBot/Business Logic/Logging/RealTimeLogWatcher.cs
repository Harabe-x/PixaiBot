using System;
using System.IO;
using PixaiBot.Business_Logic.Data_Management;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Business_Logic.Logging;

public class RealTimeLogWatcher : IRealTimeLogWatcher
{
    #region Ctor
    
    public RealTimeLogWatcher(ILogger logger)
    {
        _logger = logger; 
        
        
         _fileSystemWatcher = new FileSystemWatcher
        {
            Path = InitialConfiguration.BotLogsPath,
            EnableRaisingEvents = true 
            
        };          

        _fileSystemWatcher.Changed += OnLogFileChanged;
    }

    #endregion



    #region Methods
    
    private void OnLogFileChanged(object sender, FileSystemEventArgs e)
    {
        try
        {
            var readedText = File.ReadAllText(e.FullPath);
            
            LogFileChanged?.Invoke(this, readedText);
            
            _fileSystemWatcher.Dispose();
            
            _fileSystemWatcher = new FileSystemWatcher
            {
                Path = InitialConfiguration.BotLogsPath,
                EnableRaisingEvents = true 
            
            };          

            _fileSystemWatcher.Changed += OnLogFileChanged;
        }
        catch (IOException ioEx)
        {
            _logger.Log($"IO Error reading the log file: {ioEx.Message}", _logger.ApplicationLogFilePath);
        }
        catch (UnauthorizedAccessException uaEx)
        {
            _logger.Log($"Access denied reading the log file: {uaEx.Message}", _logger.ApplicationLogFilePath);
        }
        catch (Exception ex)
        {
            _logger.Log($"Unexpected error reading the log file: {ex.Message}", _logger.ApplicationLogFilePath);
        }
    }

    #endregion
    
    #region Fields

    public event EventHandler<string>? LogFileChanged;

    private readonly ILogger _logger;

    private FileSystemWatcher _fileSystemWatcher;

    #endregion
}
