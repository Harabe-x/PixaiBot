using PixaiBot.Data.Interfaces;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using PixaiBot.UI.Base;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Notification.Wpf;
using PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy;
using PixaiBot.Business_Logic.Extension;
using PixaiBot.UI.Models;
using Brush = System.Drawing.Brush;

namespace PixaiBot.UI.ViewModel;

public class CreditClaimerViewModel : BaseViewModel
{


    #region Commands
    
    public ICommand ClaimCreditsCommand { get; }

    #endregion  

    #region Constructor

    public CreditClaimerViewModel(ICreditClaimer creditClaimer, IToastNotificationSender notificationSender,
        IAccountsManager accountManager, IConfigManager configManager, IBotStatisticsManager botStatisticsManager,
        ILogger logger)
    {
        _creditClaimerModel = new CreditClaimerModel();

        ClaimCreditsCommand = new RelayCommand(_ => ClaimCredits());

        _accountsManager = accountManager;
        _configManager = configManager;
        _botStatisticsManager = botStatisticsManager;
        _creditClaimer = creditClaimer;
        _notificationSender = notificationSender;
        _logger = logger;

        _creditClaimer.CreditsClaimed += SendNotification;
        _creditClaimer.ErrorOccurred += SendNotification;
        _creditClaimer.ProcessStartedForAccount += UpdateBotOperationStatus;
        _botStatisticsManager.StatisticsChanged += GetFreshStatistic;
        _creditClaimerModel.BotStatistics = _botStatisticsManager.GetStatistics();

        ClaimButtonText = "Start Claiming";
        OperationStatus = "Idle.";
        
        if (!_configManager.GetConfig().CreditsAutoClaim) return;
        
        ClaimButtonText = "Stop";
        ClaimCredits();
        _creditClaimerTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromHours(AutoCreditsClaimInterval)
        };
        _creditClaimerTimer.Tick += (sender, args) => { ClaimCredits(); };


    }

    #endregion

    #region Methods

    private async void ClaimCredits()
    {
        _logger.Log("Credits claiming process started", _logger.ApplicationLogFilePath);
        if (IsRunning)
        {
            StopClaiming();
            return;
        }

        var config = _configManager.GetConfig();
        _tokenSource = new CancellationTokenSource();

        if (config.ToastNotifications) _notificationSender.SendNotification("PixaiBot", "Credits claiming process started", NotificationType.Information);

        ClaimButtonText = "Stop";
        IsRunning = true;

        IDriverCreationStrategy driverCreationStrategy = config.HeadlessBrowser
            ? new HeadlessDriverCreationStrategy()
            : new HiddenDriverCreationStrategy();

        if (config.MultiThreading)  
        {
            _logger.Log("Multi-threading enabled\nCreating a tasks to do", _logger.ApplicationLogFilePath);
            var accounts = _accountsManager.GetAllAccounts().SplitList(config.NumberOfThreads);

            var tasks = accounts.Select(account =>
                Task.Run(() => { _creditClaimer.ClaimCreditsForAllAccounts(account, driverCreationStrategy, _tokenSource.Token); },
                    _tokenSource.Token));
            await Task.WhenAll(tasks);
        }
        else
        {
            await Task.Run(
                () =>
                {
                    _creditClaimer.ClaimCreditsForAllAccounts(_accountsManager.GetAllAccounts(), driverCreationStrategy, _tokenSource.Token);
                }, _tokenSource.Token);
        }
        


            StopClaiming();
    }


    private void StopClaiming()
    {
        IsRunning = false;
        ClaimButtonText = "Start Claiming";
        OperationStatus = "Idle.";
        LastCreditClaimDate = DateTime.Now.ToString("d");
        if (_configManager.GetConfig().ToastNotifications) _notificationSender.SendNotification("PixaiBot", "Credits claiming process ended", NotificationType.Information);
        _tokenSource.Cancel();
        _logger.Log("Credits claiming process ended", _logger.ApplicationLogFilePath);
    }

    private void UpdateBotOperationStatus(object? sender, UserAccount e)
    {
        OperationStatus = $"Claiming credits for {e.Email}";
    }

    private void SendNotification(object? sender, UserAccount e)
    {
        if (_configManager.GetConfig().ToastNotifications)
            _notificationSender.SendNotification("PixaiBot", $"Claimed credits for : {e.Email}",
                NotificationType.Success);
    }

    private void SendNotification(object? sender, string e)
    {
        if (_configManager.GetConfig().ToastNotifications)
            _notificationSender.SendNotification("PixaiBot", e, NotificationType.Error);
    }

    private void GetFreshStatistic(object? sender, EventArgs e)
    {
        _creditClaimerModel.BotStatistics = _botStatisticsManager.GetStatistics();
        OnPropertyChanged();
    }

    #endregion

    #region Fields

    private CancellationTokenSource _tokenSource;

    private readonly CreditClaimerModel _creditClaimerModel;

    private readonly IAccountsManager _accountsManager;

    private readonly IBotStatisticsManager _botStatisticsManager;

    private readonly IConfigManager _configManager;

    private readonly ICreditClaimer _creditClaimer;

    private readonly ILogger _logger;

    private readonly IToastNotificationSender _notificationSender;

    private const int AutoCreditsClaimInterval = 24;

    private readonly DispatcherTimer _creditClaimerTimer;


    public bool IsRunning
    {
        get => _creditClaimerModel.IsRunning;
        set
        {
            _creditClaimerModel.IsRunning = value;
            OnPropertyChanged();
        }
    }

    public string BotVersion
    {
        get => $"Bot Version : {_creditClaimerModel.BotStatistics.BotVersion}";
        set
        {
            _creditClaimerModel.BotStatistics.BotVersion = value;
            _botStatisticsManager.SaveStatistics(_creditClaimerModel.BotStatistics);
            OnPropertyChanged();
        }
    }

    public string AccountsCount
    {
        get => $"Accounts Count : {_creditClaimerModel.BotStatistics.AccountsCount.ToString()}";
        set
        {
            _creditClaimerModel.BotStatistics.AccountsCount = int.Parse(value);
            _botStatisticsManager.SaveStatistics(_creditClaimerModel.BotStatistics);
            OnPropertyChanged();
        }
    }

    public string LastCreditClaimDate
    {
        get => $"Last credits claim date: {_creditClaimerModel.BotStatistics.LastCreditClaimDateTime:d}";
        set
        {
            _creditClaimerModel.BotStatistics.LastCreditClaimDateTime = DateTime.Parse(value);
            _botStatisticsManager.SaveStatistics(_creditClaimerModel.BotStatistics);
            OnPropertyChanged();
        }
    }

    public string ClaimButtonText
    {
        get => _creditClaimerModel.ClaimButtonText;
        set
        {
            _creditClaimerModel.ClaimButtonText = value;
            OnPropertyChanged();
        }
    }

    public string OperationStatus
    {
        get => _creditClaimerModel.OperationStatus;
        set
        {
            _creditClaimerModel.OperationStatus = value;
            OnPropertyChanged();
        }
    }

    #endregion
}