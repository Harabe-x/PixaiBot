using System;

namespace PixaiBot.Data.Interfaces;

public interface IRealTimeLogWatcher
{
    event EventHandler<string> LogFileChanged;
}