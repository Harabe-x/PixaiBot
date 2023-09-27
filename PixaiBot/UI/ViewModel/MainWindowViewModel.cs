using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PixaiBot.Data.Interfaces;
using System.Drawing;
using System.Windows.Forms;
using PixaiBot.UI.Base;

namespace PixaiBot.UI.ViewModel;

public class MainWindowViewModel : BaseViewModel, ITrayIconHelper, IWindowHelper
{
    public ICommand NavigateToDashboardCommand { get; }

    public ICommand NavigateToSettingsCommand { get; }

    public ICommand ExitApplicationCommand { get; }

    public ICommand HideApplicationCommand { get; }

    public MainWindowViewModel(INavigationService navService, ILogger logger)
    {
        _logger = logger;
        NavigateToDashboardCommand = new RelayCommand((obj) => NavigateToDashboard());
        NavigateToSettingsCommand = new RelayCommand((obj) => NavigateToSettings());
        ExitApplicationCommand = new RelayCommand((obj) => ExitApplication());
        Navigation = navService;
        HideApplicationCommand = new RelayCommand((obj) => HideApplication());
        NavigateToDashboardCommand.Execute(null);
    }

    private readonly ILogger _logger;

    private INavigationService _navigation;

    public INavigationService Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }


    private void NavigateToDashboard()
    {
        Navigation.NavigateTo<DashboardControlViewModel>();
        _logger.Log("Navigated to Dashboard", _logger.ApplicationLogFilePath);
    }

    private void NavigateToSettings()
    {
        Navigation.NavigateTo<SettingsControlViewModel>();
        _logger.Log("Navigated to Settings", _logger.ApplicationLogFilePath);
    }

    private void HideApplication()
    {
        HideToTray?.Invoke();
        _logger.Log("Hided application to tray", _logger.ApplicationLogFilePath);
    }

    private void ExitApplication()
    {
        _logger.Log("=====Application Closed=====", _logger.ApplicationLogFilePath);

        Close?.Invoke();
    }

    public bool CanHideToTray()
    {
        return true;
    }

    public bool CanCloseWindow()
    {
        return true;
    }

    public Action Close { get; set; }

    public Action HideToTray { get; set; }
}