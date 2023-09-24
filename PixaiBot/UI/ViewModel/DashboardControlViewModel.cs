﻿using PixaiBot.Data.Interfaces;
using System.Windows.Threading;
using System.Threading.Tasks;
using PixaiBot.Data.Models;
using System.Windows.Input;
using PixaiBot.UI.Base;
using System.Linq;
using System;

namespace PixaiBot.UI.ViewModel;

internal class DashboardControlViewModel : BaseViewModel
{
    public ICommand ClaimCreditsCommand { get; }

    public DashboardControlViewModel(ICreditClaimer creditClaimer, IAccountsManager accountsManager,
        IAccountsStatisticsManager accountsStatisticsManager, ILogger logger, IConfigManager configManager,
        IToastNotificationSender toastNotificationSender)
    {
        _configManager = configManager;
        _logger = logger;
        _accountsStatisticsManager = accountsStatisticsManager;
        _creditClaimer = creditClaimer;
        _accountsManager = accountsManager;
        _toastNotificationSender = toastNotificationSender;
        ClaimCreditsCommand = new RelayCommand((obj) => ClaimCreditsInNewThread());
        StartStatisticsRefreshing();
        if(_userConfig.CreditsAutoClaim) StartAutoClaim();
    }

    private readonly IAccountsManager _accountsManager;

    private readonly ICreditClaimer _creditClaimer;

    private readonly IAccountsStatisticsManager _accountsStatisticsManager;

    private readonly IConfigManager _configManager;

    private readonly ILogger _logger;

    private readonly IToastNotificationSender _toastNotificationSender;

    private string? _accountCount;

    private UserConfig _userConfig;

    public string? AccountCount
    {
        get => _accountCount;
        set
        {
            _accountCount = $"Accounts Count : {value}";
            OnPropertyChanged();
        }
    }

    private string? _accountWithClaimedCredits;

    public string? AccountWithClaimedCredits
    {
        get => _accountWithClaimedCredits;
        set
        {
            _accountWithClaimedCredits = $"Accounts With Claimed Credits : {value}";
            OnPropertyChanged();
        }
    }

    private string? _accountWithUnclaimedCredits;

    public string? AccountWithUnclaimedCredits
    {
        get => _accountWithUnclaimedCredits;
        set
        {
            _accountWithUnclaimedCredits = $"Accounts With Unclaimed credits: {value}";
            OnPropertyChanged();
        }
    }

    private void StartStatisticsRefreshing()
    {
        var refreshStatisticsTimer = new DispatcherTimer();
        refreshStatisticsTimer.Interval = TimeSpan.FromSeconds(5);
        refreshStatisticsTimer.Tick += UpdateStatistics;
        refreshStatisticsTimer.Start();
        UpdateStatistics(null, null);
    }

    private void ClaimCreditsInNewThread()
    {
        var creditClaimTask = new Task(ClaimCredits);
        creditClaimTask.Start();
    }

    private void StartAutoClaim()
    {
        var autoClaimTimer = new DispatcherTimer();
        autoClaimTimer.Interval = TimeSpan.FromHours(24);
        autoClaimTimer.Tick += (sender, args) => ClaimCreditsInNewThread();
        autoClaimTimer.Start();
        
        ClaimCreditsInNewThread();
    }

    private void ClaimCredits()
    {
        _logger.Log("Started credit claiming process ", _logger.ApplicationLogFilePath);

        var accounts = _accountsManager.GetAllAccounts().ToList();
        
        foreach (var account in accounts)
            if (_userConfig.ToastNotifications)
                _creditClaimer.ClaimCredits(account, _toastNotificationSender);
            else
                _creditClaimer.ClaimCredits(account);
    }

    private void UpdateStatistics(object? sender, EventArgs? e)
    {
        _accountsStatisticsManager.RefreshStatistics();
        AccountCount = _accountsStatisticsManager.AccountsNumber.ToString();
        AccountWithClaimedCredits = _accountsStatisticsManager.AccountsWithClaimedCredits.ToString();
        AccountWithUnclaimedCredits = _accountsStatisticsManager.AccountsWithUnclaimedCredits.ToString();
        _userConfig = _configManager.GetConfig();
        _logger.Log("Statistics refreshed", _logger.ApplicationLogFilePath);
    }

}