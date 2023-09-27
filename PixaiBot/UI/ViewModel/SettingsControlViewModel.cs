using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
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

    private readonly string? _executablePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;

    public SettingsControlViewModel(IDialogService dialogService, IAccountsManager
        accountsManager, IDataValidator dataValidator, IAccountLoginChecker
        accountLoginChecker, IBotStatisticsManager botStatisticsManager, IConfigManager configManager)
    {
        _configManager = configManager;
        _botStatisticsManager = botStatisticsManager;
        _dialogService = dialogService;
        _accountsManager = accountsManager;
        _dataValidator = dataValidator;
        _accountLoginChecker = accountLoginChecker;
        ShowAddAccountWindowCommand = new RelayCommand((obj) => ShowAddAccountWindow());
        AddManyAccountsCommand = new RelayCommand((obj) => AddManyAccounts());
        CheckAllAccountsLoginCommand = new RelayCommand((obj) => CheckAllAccountsLogin());
        StartWithSystemCommand = new RelayCommand((obj) => StartWithSystem());
        InitializeUserConfig();
    }

    private readonly IDialogService _dialogService;

    private readonly IAccountsManager _accountsManager;

    private readonly IDataValidator _dataValidator;

    private readonly IAccountLoginChecker _accountLoginChecker;

    private readonly IBotStatisticsManager _botStatisticsManager;

    private readonly IConfigManager _configManager;

    private UserConfig _userConfig;

    private bool _shouldStartWithSystem;

    public bool ShouldStartWithSystem
    {
        get => _shouldStartWithSystem;
        set
        {
            _userConfig.StartWithSystem = value;
            _shouldStartWithSystem = value;
            OnPropertyChanged();
            SaveUserConfig();
        }
    }

    private bool _enableToastNotifications;

    public bool EnableToastNotifications
    {
        get => _enableToastNotifications;
        set
        {
            _userConfig.ToastNotifications = value;
            _enableToastNotifications = value;
            OnPropertyChanged();
            SaveUserConfig();
        }
    }

    private bool _autoClaimCredits;

    public bool AutoClaimCredits
    {
        get => _autoClaimCredits;
        set
        {
            _userConfig.CreditsAutoClaim = value;
            _autoClaimCredits = value;
            OnPropertyChanged();
            SaveUserConfig();
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

    private void CheckAllAccountsLogin()
    {
        var accounts = _accountsManager.GetAllAccounts().ToList();

        var validAccountsCount = _accountLoginChecker.CheckAllAccountsLogin(accounts);

        _botStatisticsManager.ResetNumberOfAccounts();

        _botStatisticsManager.IncreaseAccountsCount(validAccountsCount);
    }

    private void InitializeUserConfig()
    {
        _userConfig = _configManager.GetConfig();
        ShouldStartWithSystem = _userConfig.StartWithSystem;
        EnableToastNotifications = _userConfig.ToastNotifications;
        AutoClaimCredits = _userConfig.CreditsAutoClaim;
    }

    private void StartWithSystem()
    {
        var rk = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        if (ShouldStartWithSystem)
            rk.SetValue("PixaiBot", _executablePath);
        else
            rk.DeleteValue("PixaiBot", false);
    }

    public void SaveUserConfig()
    {
        _configManager.SaveConfig(_userConfig);
    }
}