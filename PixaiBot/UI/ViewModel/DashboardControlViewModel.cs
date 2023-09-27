using PixaiBot.Data.Interfaces;
using System.Windows.Threading;
using System.Threading.Tasks;
using PixaiBot.Data.Models;
using System.Windows.Input;
using PixaiBot.UI.Base;
using System.Linq;
using System;
using System.Windows;

namespace PixaiBot.UI.ViewModel;

public class DashboardControlViewModel : BaseViewModel
{
    public ICommand ClaimCreditsCommand { get; }

    public DashboardControlViewModel(ICreditClaimer creditClaimer, IAccountsManager accountsManager,
        IBotStatisticsManager botStatisticsManager, ILogger logger, IConfigManager configManager,
        IToastNotificationSender toastNotificationSender)
    {
        _configManager = configManager;
        _logger = logger;
        _botStatisticsManager = botStatisticsManager;
        _creditClaimer = creditClaimer;
        _accountsManager = accountsManager;
        _toastNotificationSender = toastNotificationSender;
        ClaimCreditsCommand = new RelayCommand((obj) => ClaimCreditsInNewThread());
        StartBotDataRefreshing();
        if (_userConfig.CreditsAutoClaim) StartCreditsAutoClaim();
    }

    private const int CreditClaimerInterval = 24;

    private const int StatisticsRefreshInterval = 5;

    private readonly IAccountsManager _accountsManager;

    private readonly ICreditClaimer _creditClaimer;

    private readonly IBotStatisticsManager _botStatisticsManager;

    private readonly IConfigManager _configManager;

    private readonly ILogger _logger;

    private readonly IToastNotificationSender _toastNotificationSender;

    private string? _accountCount;

    private UserConfig _userConfig;

    private string _creditClaimerInfo;

    public string CreditClaimerInfo
    {
        get => _creditClaimerInfo;
        set
        {
            _creditClaimerInfo = value;
            OnPropertyChanged();
        }
    }


    public string? AccountCount
    {
        get => _accountCount;
        set
        {
            _accountCount = $"Accounts Count : {value}";
            OnPropertyChanged();
        }
    }

    private string? _lastCreditClaimDateTime;

    public string? LastCreditClaimDateTime
    {
        get => _lastCreditClaimDateTime;
        set
        {
            _botStatisticsManager.SetClaimDateTime(DateTime.Parse(value));
            _lastCreditClaimDateTime = $"Last Credit Claim Date :  {value}";
            OnPropertyChanged();
        }
    }

    private string? _botVersion;

    public string? BotVersion
    {
        get => _botVersion;
        set
        {
            _botVersion = $"Bot Version: {value}";
            OnPropertyChanged();
        }
    }

    private void StartBotDataRefreshing()
    {
        var refreshStatisticsTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(StatisticsRefreshInterval)
        };

        refreshStatisticsTimer.Tick += RefreshBotData;
        refreshStatisticsTimer.Start();

        RefreshBotData(null, null);
    }

    private void ClaimCreditsInNewThread()
    {
        var creditClaimTask = new Task(ClaimCredits);
        creditClaimTask.Start();
    }

    private void StartCreditsAutoClaim()
    {
        var autoClaimTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromHours(CreditClaimerInterval)
        };

        autoClaimTimer.Tick += (sender, args) => ClaimCreditsInNewThread();
        autoClaimTimer.Start();

        ClaimCreditsInNewThread();
    }

    private void ClaimCredits()
    {
        _logger.Log("Started credit claiming process ", _logger.ApplicationLogFilePath);
        
        var accounts = _accountsManager.GetAllAccounts().ToList();
        foreach (var account in accounts)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                CreditClaimerInfo = $"Claiming Credits on {account.Email}";
            });
            
            if (_userConfig.ToastNotifications)
            {
                _creditClaimer.ClaimCredits(account, _toastNotificationSender);
            }
            else
            {
                _creditClaimer.ClaimCredits(account);
            }
        }
        
        Application.Current.Dispatcher.Invoke(() =>
        {
            CreditClaimerInfo = $"Credits Claimed!";
            LastCreditClaimDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            
        });

    }

    private void RefreshBotData(object? sender, EventArgs? e)
    {
        _botStatisticsManager.RefreshStatistics();
        AccountCount = _botStatisticsManager.AccountsNumber.ToString();
        LastCreditClaimDateTime = _botStatisticsManager.LastCreditClaimDateTime.ToString();
        BotVersion = _botStatisticsManager.BotVersion;
        _userConfig = _configManager.GetConfig();
        _logger.Log("Data refreshed", _logger.ApplicationLogFilePath);
    }

}