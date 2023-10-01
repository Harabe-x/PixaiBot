using System;
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
        accountLoginChecker, IBotStatisticsManager botStatisticsManager, IConfigManager configManager, IToastNotificationSender toastNotificationSender)
    {
        _toastNotificationSender = toastNotificationSender;
        _configManager = configManager;
        _botStatisticsManager = botStatisticsManager;
        _dialogService = dialogService;
        _accountsManager = accountsManager;
        _dataValidator = dataValidator;
        _accountLoginChecker = accountLoginChecker;
        ShowAddAccountWindowCommand = new RelayCommand((obj) => ShowAddAccountWindow());
        AddManyAccountsCommand = new RelayCommand((obj) => AddManyAccounts());
        CheckAllAccountsLoginCommand = new RelayCommand((obj) => CheckAllAccountsLoginInNewThread());
        StartWithSystemCommand = new RelayCommand((obj) => StartWithSystem());
        UpdateToastNotificationPreferenceCommand = new RelayCommand((obj) => UpdateToastNotificationPreference());
        _configManager.ConfigChanged += UserChangedSettings;
        InitializeUserConfig();
    }

    private readonly IDialogService _dialogService;

    private readonly IAccountsManager _accountsManager;

    private readonly IDataValidator _dataValidator;

    private readonly IAccountLoginChecker _accountLoginChecker;

    private readonly IBotStatisticsManager _botStatisticsManager;

    private readonly IConfigManager _configManager;

    private readonly IToastNotificationSender _toastNotificationSender;


    private bool _shouldStartWithSystem;

    public bool ShouldStartWithSystem
    {
        get => _shouldStartWithSystem;
        set
        {
            _shouldStartWithSystem = value;
            _configManager.SetStartWithSystemFlag(value);
        }
    }

    private bool _enableToastNotifications;

    public bool EnableToastNotifications
    {
        get => _enableToastNotifications;
        set
        {
            _enableToastNotifications = value;
            _configManager.SetToastNotificationsFlag(value);
        }
    }

    private bool _autoClaimCredits;

    public bool AutoClaimCredits
    {
        get => _autoClaimCredits;
        set
        {
            _autoClaimCredits = value;
            _configManager.SetCreditsAutoClaimFlag(value);
        }
    }

    private void UserChangedSettings(object? sender, EventArgs e)
    {
        OnPropertyChanged();
    }

    private void UpdateToastNotificationPreference()
    {
        if (EnableToastNotifications)
        {
            _toastNotificationSender.SendNotification("PixaiBot", "Toast Notifications enabled,Now you will recive notifcations", NotificationType.Information);
        }
        else
        {
            _toastNotificationSender.SendNotification("PixaiBot", "Toast Notifications disabled,Now you won't receive notifications", NotificationType.Information);
        }
    }

    private void ShowAddAccountWindow()
    {
        _dialogService.ShowDialog(new AddAccountWindowView(_accountsManager, _dataValidator), true);
    }

    private void AddManyAccounts()
    {
        _accountsManager.AddManyAccounts();
    }

    private void CheckAllAccountsLoginInNewThread()
    {
        var task = new Task(CheckAllAccountsLogin);
        task.Start();
    }

    private void CheckAllAccountsLogin()
    {
        var accounts = _accountsManager.GetAllAccounts().ToList();

        var validAccountsCount = 0;

        validAccountsCount = EnableToastNotifications ? _accountLoginChecker.CheckAllAccountsLogin(accounts, _toastNotificationSender) : _accountLoginChecker.CheckAllAccountsLogin(accounts);

        _botStatisticsManager.ResetNumberOfAccounts();

        _botStatisticsManager.IncreaseAccountsCount(validAccountsCount);
    }


    private void InitializeUserConfig()
    {
        ShouldStartWithSystem = _configManager.ShouldStartWithSystem;
        EnableToastNotifications = _configManager.ShouldSendToastNotifications;
        AutoClaimCredits = _configManager.ShouldAutoClaimCredits;
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