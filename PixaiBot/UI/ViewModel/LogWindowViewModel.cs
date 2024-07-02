using System.Reflection.Metadata;
using System.Windows;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;
using PixaiBot.UI.Models;

namespace PixaiBot.UI.ViewModel;

public class LogWindowViewModel : BaseViewModel 
{
    public LogWindowViewModel(IRealTimeLogWatcher logWatcher)
    {
        _logWindowModel = new LogWindowModel();
        _logWatcher = logWatcher;
        _logWatcher.LogFileChanged += SetLog;
    }



    public void SetLog(object? sender,string e)
    {
        Application.Current.Dispatcher.Invoke(() => { Log = e; });
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
 
    private readonly LogWindowModel _logWindowModel;

    private readonly IRealTimeLogWatcher _logWatcher; 
}