﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using Notification.Wpf;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;
using PixaiBot.UI.Base;
using PixaiBot.UI.View;

namespace PixaiBot.UI.ViewModel;

public class SettingsControlViewModel : BaseViewModel
{
    public ICommand ShowAddAccountWindowCommand { get; }

    public ICommand AddManyAccountsCommand { get; }

    public ICommand CheckAllAccountsLoginCommand { get; }

    public ICommand StartWithSystemCommand { get; }

    public ICommand UpdateToastNotificationPreferenceCommand { get; }

    private readonly string? _executablePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;

    public SettingsControlViewModel(IDialogService dialogService, IAccountsManager
        accountsManager, IDataValidator dataValidator, IAccountLoginChecker
        accountLoginChecker,ILogger logger,IBotStatisticsManager botStatisticsManager, IConfigManager configManager, IToastNotificationSender toastNotificationSender)
    {
        _toastNotificationSender = toastNotificationSender;
        _configManager = configManager;
        _botStatisticsManager = botStatisticsManager;
        _dialogService = dialogService;
        _logger = logger;
        _accountsManager = accountsManager;
        _dataValidator = dataValidator;
        _accountLoginChecker = accountLoginChecker;
        ShowAddAccountWindowCommand = new RelayCommand((obj) => ShowAddAccountWindow());
        AddManyAccountsCommand = new RelayCommand((obj) => AddManyAccounts());
        CheckAllAccountsLoginCommand = new RelayCommand((obj) => CheckAllAccountsLogin());
        StartWithSystemCommand = new RelayCommand((obj) => StartWithSystem());
        UpdateToastNotificationPreferenceCommand = new RelayCommand((obj) => UpdateToastNotificationPreference());
        _userConfig = _configManager.GetConfig();
    }

    private readonly IDialogService _dialogService;

    private readonly IAccountsManager _accountsManager;

    private readonly IDataValidator _dataValidator;

    private readonly IAccountLoginChecker _accountLoginChecker;

    private readonly IBotStatisticsManager _botStatisticsManager;

    private readonly IConfigManager _configManager;

    private readonly IToastNotificationSender _toastNotificationSender;

    private readonly ILogger _logger;

    private readonly UserConfig _userConfig;

    public bool ShouldStartWithSystem
    {
        get => _userConfig.StartWithSystem;
        set
        {
            _userConfig.StartWithSystem = value;
            _configManager.SaveConfig(_userConfig);
            OnPropertyChanged();
        }
    }
    
    public bool EnableToastNotifications
    {
        get => _userConfig.ToastNotifications;
        set
        {
            _userConfig.ToastNotifications = value;
            _configManager.SaveConfig(_userConfig);
            OnPropertyChanged();
        }
    }

    public bool AutoClaimCredits
    {
        get => _userConfig.CreditsAutoClaim;
        set
        {
            _userConfig.CreditsAutoClaim = value;
            _configManager.SaveConfig(_userConfig);
            OnPropertyChanged();
        }
    }

    private void UpdateToastNotificationPreference()
    {
        _toastNotificationSender.SendNotification("PixaiBot",
            EnableToastNotifications
                ? "Toast Notifications enabled,Now you will receive notifications"
                : "Toast Notifications disabled,Now you won't receive notifications",
                NotificationType.Information);
    }

    private void ShowAddAccountWindow()
    {
        _dialogService.ShowDialog(new AddAccountWindowView(),new AddAccountWindowViewModel(_accountsManager,_dataValidator,_logger),true);
    }

    private void AddManyAccounts()
    {
        _accountsManager.AddManyAccounts();
    }

    private void CheckAllAccountsLogin()
    {
        var accountCheckTask = new Task(() =>
        {
            var accounts = _accountsManager.GetAllAccounts().ToList();

            var validAccountsCount = 0;

            validAccountsCount = EnableToastNotifications
                ? _accountLoginChecker.CheckAllAccountsLogin(accounts, _toastNotificationSender)
                : _accountLoginChecker.CheckAllAccountsLogin(accounts);

            _botStatisticsManager.ResetNumberOfAccounts();

            _botStatisticsManager.IncreaseAccountsCount(validAccountsCount);
        });

        accountCheckTask.Start();
    }

 
    private void StartWithSystem()
    {
        var registryKey = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        if (ShouldStartWithSystem)
            registryKey?.SetValue("PixaiBot", _executablePath);
        else
            registryKey?.DeleteValue("PixaiBot", false);
    }
}