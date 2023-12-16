using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PixaiBot.Data.Interfaces;
using System.Drawing;
using System.Windows.Forms;
using Notification.Wpf;
using OpenQA.Selenium.DevTools;
using PixaiBot.UI.Base;
using ICommand = System.Windows.Input.ICommand;

namespace PixaiBot.UI.ViewModel;

public class MainWindowViewModel : BaseViewModel, ITrayIconHelper, IWindowHelper
{
    #region Commands

    public ICommand NavigateToDashboardCommand { get; }

    public ICommand NavigateToSettingsCommand { get; }

    public ICommand NavigateToAccountsListCommand { get; }

    public ICommand NavigateToLogAccountInfoCommand { get; }

    public ICommand NavigateToAccountCreatorCommand { get; }

    public ICommand ExitApplicationCommand { get; }

    public ICommand HideApplicationCommand { get; }


    #endregion
    #region Constructor

    public MainWindowViewModel(ITcpServerConnector tcpServerConnector, INavigationService navService, ILogger logger, IToastNotificationSender toastNotificationASender, IConfigManager configManager)
    {
        _configManager = configManager;
        _toastNotificationSender = toastNotificationASender;
        _logger = logger;
        Navigation = navService;
        _tcpServerConnector = tcpServerConnector;
        NavigateToAccountCreatorCommand = new RelayCommand((obj) => NavigateToAccountCreator());
        NavigateToLogAccountInfoCommand = new RelayCommand((obj) => NavigateToLogAccountInfo());
        NavigateToDashboardCommand = new RelayCommand((obj) => NavigateToDashboard());
        NavigateToSettingsCommand = new RelayCommand((obj) => NavigateToSettings());
        ExitApplicationCommand = new RelayCommand((obj) => ExitApplication());
        HideApplicationCommand = new RelayCommand((obj) => HideApplication());
        NavigateToAccountsListCommand = new RelayCommand((obj) => NavigateToAccountsList());
        NavigateToDashboardCommand.Execute(null);
    }

    #endregion
    #region Methods

    private void NavigateToAccountsList()
    {
        _tcpServerConnector.SendMessage("mNavigating to Account list");
        Navigation.NavigateTo<AccountListControlViewModel>();
        _logger.Log("Navigated to Accounts List control", _logger.ApplicationLogFilePath);
    }

    private void NavigateToLogAccountInfo()
    {
        _tcpServerConnector.SendMessage("mNavigating to Account info logger");

        Navigation.NavigateTo<LogAccountInfoControlViewModel>();
        _logger.Log("Navigated to Accounts Logger control", _logger.ApplicationLogFilePath);
    }

    private void NavigateToAccountCreator()
    {
        _tcpServerConnector.SendMessage("mNavigating to Account Creator");

        Navigation.NavigateTo<AccountCreatorControlViewModel>();
        _logger.Log("Navigated to Account Creator control", _logger.ApplicationLogFilePath);
    }


    private void NavigateToDashboard()
    {
        _tcpServerConnector.SendMessage("mNavigating to Dashboard");

        Navigation.NavigateTo<DashboardControlViewModel>();
        _logger.Log("Navigated to Dashboard control", _logger.ApplicationLogFilePath);
    }

    private void NavigateToSettings()
    {
        _tcpServerConnector.SendMessage("m Navigating to Settings");

        Navigation.NavigateTo<SettingsControlViewModel>();
        _logger.Log("Navigated to Settings control", _logger.ApplicationLogFilePath);
    }

    private void HideApplication()
    {

        if (_configManager.GetConfig().ToastNotifications)
            _toastNotificationSender.SendNotification("PixaiBot", "Application minimized to system tray", NotificationType.Information);

        HideToTray?.Invoke();
        _logger.Log("Hided application to tray", _logger.ApplicationLogFilePath);

        _tcpServerConnector.SendMessage("c Application Hided");

    }

    private void ExitApplication()
    {
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

    #endregion
    #region Fields

    public Action Close { get; set; }

    public Action HideToTray { get; set; }

    private readonly ILogger _logger;

    private readonly IToastNotificationSender _toastNotificationSender;

    private readonly IConfigManager _configManager;

    private readonly ITcpServerConnector _tcpServerConnector;

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

    #endregion


}