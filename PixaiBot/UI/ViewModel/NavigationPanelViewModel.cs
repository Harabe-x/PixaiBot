﻿using System;
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

public class NavigationPanelViewModel : BaseViewModel, ITrayIconHelper, IWindowHelper
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

    public NavigationPanelViewModel(INavigationService navService, ILogger logger, IToastNotificationSender toastNotificationASender, IConfigManager configManager)
    {
        _configManager = configManager;
        _toastNotificationSender = toastNotificationASender;
        _logger = logger;
        Navigation = navService;
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

    private void HideApplication()
    {

        if (_configManager.GetConfig().ToastNotifications) _toastNotificationSender.SendNotification("PixaiBot", "Application minimized to system tray, click this notification to maximize application", NotificationType.Information, () => { ShowWindow?.Invoke(); });
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

    private INavigationService _navigation;

    private readonly IConfigManager _configManager;

    private readonly ILogger _logger;

    public Action ShowWindow { get; set; }

    public Action Close { get; set; }

    public Action HideToTray { get; set; }

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