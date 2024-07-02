    using System;
using System.Windows.Input;
using Notification.Wpf;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;

namespace PixaiBot.UI.ViewModel;

public class NavigationPanelViewModel : BaseViewModel, ITrayIconHelper, IWindowHelper
{
    #region Constructor
    

    public NavigationPanelViewModel(INavigationService navService, ILogger logger,
        IToastNotificationSender toastNotificationASender, IConfigManager configManager)
    {
        NavigateToAccountCreatorCommand = new RelayCommand(_ => NavigateToAccountCreator());
        NavigateToLogAccountInfoCommand = new RelayCommand(obj => NavigateToLogAccountInfo());
        NavigateToDashboardCommand = new RelayCommand(_ => NavigateToDashboard());
        NavigateToSettingsCommand = new RelayCommand(_ => NavigateToSettings());
        ExitApplicationCommand = new RelayCommand(_ => ExitApplication());
        HideApplicationCommand = new RelayCommand(_ => HideApplication());
        NavigateToAccountsListCommand = new RelayCommand(_ => NavigateToAccountsList());
        NavigateToDebugToolsCommand = new RelayCommand(_ => NavigateToDebugTools());
    
        Navigation = navService;
        _configManager = configManager;
        _toastNotificationSender = toastNotificationASender;
        _logger = logger;

        NavigateToDashboardCommand.Execute(null);
    }

    #endregion
    

    #region Commands
    
    public ICommand NavigateToDashboardCommand { get; }

    public ICommand NavigateToSettingsCommand { get; }

    public ICommand NavigateToAccountsListCommand { get; }

    public ICommand NavigateToLogAccountInfoCommand { get; }

    public ICommand NavigateToAccountCreatorCommand { get; }
    
    public ICommand NavigateToDebugToolsCommand { get; }
    
    public ICommand ExitApplicationCommand { get; }

    public ICommand HideApplicationCommand { get; }

    #endregion

    #region Methods

    private void NavigateToAccountsList()
    {
        Navigation.NavigateTo<AccountListViewModel>();
        _logger.Log("Navigated to Accounts List control", _logger.ApplicationLogFilePath);
    }

    private void NavigateToLogAccountInfo()
    {
        Navigation.NavigateTo<AccountInfoLoggerViewModel>();
        _logger.Log("Navigated to Accounts Logger control", _logger.ApplicationLogFilePath);
    }

    private void NavigateToAccountCreator()
    {
        Navigation.NavigateTo<AccountCreatorViewModel>();
        _logger.Log("Navigated to Account Creator control", _logger.ApplicationLogFilePath);
    }


    private void NavigateToDashboard()
    {
        Navigation.NavigateTo<CreditClaimerViewModel>();
        _logger.Log("Navigated to Dashboard control", _logger.ApplicationLogFilePath);
    }

    private void NavigateToSettings()
    {
        Navigation.NavigateTo<SettingsViewModel>();
        _logger.Log("Navigated to Settings control", _logger.ApplicationLogFilePath);
    }

    private void NavigateToDebugTools()
    {
        Navigation.NavigateTo<DebugToolsViewModel>();
        _logger.Log("Navigated to Debug Tools", _logger.ApplicationLogFilePath);
    }

    private void HideApplication()
    {
        if (_configManager.GetConfig().ToastNotifications)
            _toastNotificationSender.SendNotification("PixaiBot",
                "Application minimized to system tray, click this notification to maximize application",
                NotificationType.Information, () => { ShowWindow?.Invoke(); });
        HideToTray?.Invoke();
        _logger.Log("Hided application to tray", _logger.ApplicationLogFilePath);
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

    public bool CanShowWindow()
    {
        return true;
    }

    #endregion

    #region Fields

    private readonly IToastNotificationSender _toastNotificationSender;

    private readonly INavigationService _navigation;

    private readonly IConfigManager _configManager;

    private readonly ILogger _logger;

    public Action ShowWindow { get; set; }

    public Action Close { get; set; }

    public Action HideToTray { get; set; }

    // ReSharper disable once MemberCanBePrivate.Global
    public INavigationService Navigation
    {
        get => _navigation;
        init
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }

    #endregion
}