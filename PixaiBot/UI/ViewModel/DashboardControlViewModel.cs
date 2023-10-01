using PixaiBot.Data.Interfaces;
using System.Windows.Threading;
using System.Threading.Tasks;
using PixaiBot.Data.Models;
using System.Windows.Input;
using PixaiBot.UI.Base;
using System.Linq;
using System;
using System.Globalization;
using System.Windows;
using PixaiBot.Bussines_Logic;

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
        
        _creditClaimer.CreditClaimed += CreditClaimed;

        _botStatisticsManager.StatisticsChanged += StatisticsRefreshed;

        StatisticsRefreshed(null, EventArgs.Empty);

        if (_configManager.ShouldAutoClaimCredits) StartCreditsAutoClaim();
    }

    private const int CreditClaimerInterval = 24;

    private readonly IAccountsManager _accountsManager;

    private readonly ICreditClaimer _creditClaimer;

    private readonly IBotStatisticsManager _botStatisticsManager;

    private readonly IConfigManager _configManager;

    private readonly ILogger _logger;

    private readonly IToastNotificationSender _toastNotificationSender;

    private string? _accountCount;

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
            _lastCreditClaimDateTime = $"Last Credit Claim Date :  {DateTime.Parse(value):dd/MM HH:mm}";
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


    private void StatisticsRefreshed(object? sender, EventArgs e)
    {
        BotVersion = _botStatisticsManager.BotVersion;
        AccountCount = _botStatisticsManager.AccountsNumber.ToString(); 
        LastCreditClaimDateTime = _botStatisticsManager.LastCreditClaimDateTime.ToString();
    }


    private void CreditClaimed(object? sender, UserAccount e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            CreditClaimerInfo = $"Claiming Credits For {e.Email}";
        });
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
        if(_configManager.ShouldSendToastNotifications)
            _creditClaimer.ClaimCreditsForAllAccounts(_accountsManager.GetAllAccounts(), _toastNotificationSender);
        else
            _creditClaimer.ClaimCreditsForAllAccounts(_accountsManager.GetAllAccounts());

        _logger.Log("Credits claimed", _logger.ApplicationLogFilePath);
       
        CreditClaimerInfo = "Credits Claimed.";

        LastCreditClaimDateTime = DateTime.Now.ToString();

    }

  
}