﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using Notification.Wpf;
using PixaiBot.Bussines_Logic.Data_Handling;
using PixaiBot.Bussines_Logic.Data_Management;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;
using PixaiBot.UI.Models;
using PixaiBot.UI.View;

namespace PixaiBot.UI.ViewModel;

//TODO: Refactor
public class SettingsViewModel : BaseViewModel
{
    #region Commands

    public ICommand ShowAddAccountWindowCommand { get; }

    public ICommand AddManyAccountsCommand { get; }

    public ICommand CheckAllAccountsLoginCommand { get; }

    public ICommand StartWithSystemCommand { get; }

    public ICommand UpdateToastNotificationPreferenceCommand { get; }

    #endregion

    #region Constructor

    public SettingsViewModel(IDialogService dialogService, IAccountsManager
            accountsManager, IDataValidator dataValidator, IAccountLoginChecker
            accountLoginChecker, ILogger logger, IBotStatisticsManager botStatisticsManager,
        IConfigManager configManager,
        IToastNotificationSender toastNotificationSender)
    {
        _toastNotificationSender = toastNotificationSender;
        _configManager = configManager;
        _botStatisticsManager = botStatisticsManager;
        _dialogService = dialogService;
        _logger = logger;
        _accountsManager = accountsManager;
        _dataValidator = dataValidator;
        _accountLoginChecker = accountLoginChecker;
        _settingsModel = new SettingsModel();
        ShowAddAccountWindowCommand = new RelayCommand((obj) => ShowAddAccountWindow());
        AddManyAccountsCommand = new RelayCommand((obj) => AddManyAccounts());
        CheckAllAccountsLoginCommand = new RelayCommand((obj) => CheckAllAccountsLogin());
        StartWithSystemCommand = new RelayCommand((obj) => StartWithSystem());
        UpdateToastNotificationPreferenceCommand = new RelayCommand((obj) => UpdateToastNotificationPreference());
        _settingsModel.UserConfig = _configManager.GetConfig();
        AccountCheckerButtonText = "Validate accounts";
    }

    #endregion

    #region Methods

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
        _dialogService.ShowDialog(new AddAccountView(),
            new AddAccountViewModel(_accountsManager, _dataValidator, _logger), true);
    }

    private void AddManyAccounts()
    {
        _accountsManager.AddManyAccounts();
    }

    private async void CheckAllAccountsLogin()
    {
        if (IsAccountCheckerRunning)
        {
            CancelAccountsChecking();
            return;
        }

        IsAccountCheckerRunning = true;
        _tokenSource = new CancellationTokenSource();
        AccountCheckerButtonText = "Stop";

        var accountsList = _accountsManager.GetAllAccounts();

        IEnumerable<UserAccount> validAccounts = null;

        await Task.Run(() =>
        {
            validAccounts = _accountLoginChecker.CheckAllAccountsLogin(accountsList.ToList(), _tokenSource.Token);
        });


        var statistics = _botStatisticsManager.GetStatistics();

        statistics.AccountsCount = accountsList.Count();


        JsonWriter.WriteJson(validAccounts, InitialConfiguration.AccountsFilePath);

        CancelAccountsChecking();
    }

    private void CancelAccountsChecking()
    {
        IsAccountCheckerRunning = false;
        AccountCheckerButtonText = "Validate accounts";
        _tokenSource.Cancel();
    }

    private void StartWithSystem()
    {
        var registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        if (ShouldStartWithSystem) registryKey?.SetValue("PixaiBot", _executablePath);
        else registryKey?.DeleteValue("PixaiBot", false);
    }

    #endregion

    #region Fields

    private readonly IDialogService _dialogService;

    private readonly string? _executablePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;

    private readonly IAccountsManager _accountsManager;

    private readonly IDataValidator _dataValidator;

    private readonly IAccountLoginChecker _accountLoginChecker;

    private readonly IBotStatisticsManager _botStatisticsManager;

    private readonly IConfigManager _configManager;

    private readonly IToastNotificationSender _toastNotificationSender;

    private readonly ILogger _logger;

    private readonly SettingsModel _settingsModel;

    private CancellationTokenSource _tokenSource;

    private const int MaxNumberOfThreads = 10;


    public string AccountCheckerButtonText
    {
        get => _settingsModel.AccountCheckerButtonText;
        set
        {
            _settingsModel.AccountCheckerButtonText = value;
            OnPropertyChanged();
        }
    }

    public bool IsAccountCheckerRunning
    {
        get => _settingsModel.IsAccountCheckerRunning;

        set
        {
            _settingsModel.IsAccountCheckerRunning = value;
            OnPropertyChanged();
        }
    }

    public bool ShouldStartWithSystem
    {
        get => _settingsModel.UserConfig.StartWithSystem;
        set
        {
            _settingsModel.UserConfig.StartWithSystem = value;
            _configManager.SaveConfig(_settingsModel.UserConfig);
            OnPropertyChanged();
        }
    }

    public bool EnableToastNotifications
    {
        get => _settingsModel.UserConfig.ToastNotifications;
        set
        {
            _settingsModel.UserConfig.ToastNotifications = value;
            _configManager.SaveConfig(_settingsModel.UserConfig);
            OnPropertyChanged();
        }
    }

    public bool AutoClaimCredits
    {
        get => _settingsModel.UserConfig.CreditsAutoClaim;
        set
        {
            _settingsModel.UserConfig.CreditsAutoClaim = value;
            _configManager.SaveConfig(_settingsModel.UserConfig);
            OnPropertyChanged();
        }
    }

    public bool MultiThreading
    {
        get => _settingsModel.UserConfig.MultiThreading;
        set
        {
            _settingsModel.UserConfig.MultiThreading = value;
            _configManager.SaveConfig(_settingsModel.UserConfig);
            OnPropertyChanged();
        }
    }

    public string NumberOfThreads
    {
        get => _settingsModel.UserConfig.NumberOfThreads.ToString();
        set
        {
            if (!int.TryParse(value, out var parsedValue) || parsedValue > MaxNumberOfThreads) return;
            _settingsModel.UserConfig.NumberOfThreads = parsedValue;
            _configManager.SaveConfig(_settingsModel.UserConfig);
            OnPropertyChanged();
        }
    }

    #endregion
}