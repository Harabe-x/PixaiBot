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

public class MainWindowViewModel : BaseViewModel, ITrayIconHelper, IWindowHelper
{
    public ICommand NavigateToDashboardCommand { get; }

    public ICommand NavigateToSettingsCommand { get; }

    public ICommand NavigateToAccountsListCommand { get; }

    public ICommand NavigateToLogAccountInfoCommand { get; }

    public ICommand NavigateToAccountCreatorCommand { get; }

    public ICommand ExitApplicationCommand { get; }

    public ICommand HideApplicationCommand { get; }

    public MainWindowViewModel(INavigationService navService, ILogger logger,IToastNotificationSender toastNotificationASender,IConfigManager configManager)
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

    private readonly ILogger _logger;

    private readonly IToastNotificationSender _toastNotificationSender;

    private readonly IConfigManager _configManager;

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

    private void NavigateToAccountsList()
    {
        Navigation.NavigateTo<AccountListControlViewModel>();
        _logger.Log("Navigated to Accounts List control", _logger.ApplicationLogFilePath);
    }

    private void NavigateToLogAccountInfo()
    {
        Navigation.NavigateTo<LogAccountInfoControlViewModel>();
        _logger.Log("Navigated to Accounts Logger control",_logger.ApplicationLogFilePath);
    }

    private void NavigateToAccountCreator()
    {
        Navigation.NavigateTo<AccountCreatorControlViewModel>();
        _logger.Log("Navigated to Account Creator control", _logger.ApplicationLogFilePath);
    }


    private void NavigateToDashboard()
    {
        Navigation.NavigateTo<DashboardControlViewModel>();
        _logger.Log("Navigated to Dashboard control", _logger.ApplicationLogFilePath);
    }

    private void NavigateToSettings()
    {
        Navigation.NavigateTo<SettingsControlViewModel>();
        _logger.Log("Navigated to Settings control", _logger.ApplicationLogFilePath);
    }

    private void HideApplication()
    {
        if(_configManager.ShouldSendToastNotifications)
            _toastNotificationSender.SendNotification("PixaiBot","Application minimized to system tray",NotificationType.Information);

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

    public Action Close { get; set; }

    public Action HideToTray { get; set; }
}