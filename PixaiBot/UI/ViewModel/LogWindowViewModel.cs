using System.Windows;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;
using PixaiBot.UI.Models;

namespace PixaiBot.UI.ViewModel;

public class LogWindowViewModel : BaseViewModel
{
    private readonly IRealTimeLogWatcher _logWatcher;

    private readonly LogWindowModel _logWindowModel;

    public LogWindowViewModel(IRealTimeLogWatcher logWatcher)
    {
        _logWindowModel = new LogWindowModel();
        _logWatcher = logWatcher;
        _logWatcher.LogFileChanged += SetLog;
    }

    public string Log
    {
        get => _logWindowModel.Log;

        set
        {
            _logWindowModel.Log = value;
            OnPropertyChanged();
        }
    }


    public void SetLog(object? sender, string e)
    {
        Application.Current.Dispatcher.Invoke(() => { Log = e; });
    }
}