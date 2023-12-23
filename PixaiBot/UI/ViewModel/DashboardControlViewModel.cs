using PixaiBot.Data.Interfaces;
using System.Windows.Threading;
using System.Threading.Tasks;
using PixaiBot.Data.Models;
using System.Windows.Input;
using PixaiBot.UI.Base;
using System.Linq;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Notification.Wpf;
using PixaiBot.Bussines_Logic;
using PixaiBot.UI.Models;
using Brush = System.Drawing.Brush;

namespace PixaiBot.UI.ViewModel;

public class DashboardControlViewModel : BaseViewModel
{
    public ICommand ClaimCreditsCommand { get; }


    public DashboardControlViewModel(ICreditClaimer creditClaimer,IToastNotificationSender notificationSender,IAccountsManager accountManager,IConfigManager configManager,IBotStatisticsManager botStatisticsManager)
    {
        _dashboardControlModel = new DashboardControlModel();

        ClaimCreditsCommand = new RelayCommand((obj) => ClaimCredits());
        _accountsManager = accountManager;
        _configManager = configManager;
        _botStatisticsManager = botStatisticsManager;
        _creditClaimer = creditClaimer;
        _notificationSender = notificationSender;
        _creditClaimer.CreditsClaimed += SendNotification;
        _creditClaimer.ProcessStartedForAccount += UpdateBotOperationStatus;
        _botStatisticsManager.StatisticsChanged += GetFreshStatistic;
        _dashboardControlModel.BotStatistics = _botStatisticsManager.GetStatistics();

        

        if (_configManager.GetConfig().CreditsAutoClaim)
        {
            ClaimCredits();
            _creditClaimerTimer = new DispatcherTimer()
            {
                Interval  = TimeSpan.FromHours(AutoCreditsClaimInterval)
            };
            _creditClaimerTimer.Tick += (sender, args) =>
            {
                ClaimCredits();
            };
        }

        ClaimButtonText = "Start Claiming";
        BotOperationStatus = "Idle.";
    }
    


    #region Methods

 

    public void ClaimCredits()
    {
        if (IsRunning)
        {
            IsRunning = false;
            ClaimButtonText = "Start Claiming";
            BotOperationStatus = "Idle.";
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            return;
        }
        else
        {
             IsRunning = true;
            _tokenSource = new CancellationTokenSource();
            ClaimButtonText = "Stop";
        }

        Task.Run(() =>
        {
            _creditClaimer.ClaimCreditsForAllAccounts(_accountsManager.GetAllAccounts(), _tokenSource.Token);
            IsRunning = false;
            ClaimButtonText = "Start Claiming";
            BotOperationStatus = "idle.";
            LastCreditClaimDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        });
    }
    private void UpdateBotOperationStatus(object? sender, UserAccount e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            //BotOperationStatus = $"Claiming credits for {e.Email}"; 
        });
    }
    private void SendNotification(object? sender, UserAccount e)
    {
        if (_configManager.GetConfig().ToastNotifications)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _notificationSender.SendNotification("PixaiBot",$"Claimed credits for : {e.Email}",NotificationType.Success);
            });
        }
    }
    private void GetFreshStatistic(object? sender, EventArgs e)
    {
        _dashboardControlModel.BotStatistics = _botStatisticsManager.GetStatistics();
        OnPropertyChanged();
    }

    
    #endregion  


    #region Fields

    private CancellationTokenSource _tokenSource;

    private readonly DashboardControlModel _dashboardControlModel;

    private readonly IAccountsManager _accountsManager;

    private readonly IBotStatisticsManager _botStatisticsManager;

    private readonly IConfigManager _configManager;

    private readonly ICreditClaimer _creditClaimer;

    private readonly IToastNotificationSender _notificationSender;

    private const int AutoCreditsClaimInterval = 24;

    private readonly DispatcherTimer _creditClaimerTimer;

 


    public bool IsRunning
    {
        get => _dashboardControlModel.IsRunning;
        set
        {
            _dashboardControlModel.IsRunning = value;
            OnPropertyChanged();
        }
    }

    public string BotVersion
    {
        get => $"Bot Version : {_dashboardControlModel.BotStatistics.BotVersion}";
        set
        {
            _dashboardControlModel.BotStatistics.BotVersion = value;
            _botStatisticsManager.SaveStatistics(_dashboardControlModel.BotStatistics);
            OnPropertyChanged();
        }
    }

    public string AccountsCount
    {
        get => $"Accounts Count : {_dashboardControlModel.BotStatistics.AccountsCount.ToString()}";
        set     
        {
            _dashboardControlModel.BotStatistics.AccountsCount = int.Parse(value);
            _botStatisticsManager.SaveStatistics(_dashboardControlModel.BotStatistics);
            OnPropertyChanged();
        }
    }

    public string LastCreditClaimDate
    {
        get => $"Last credits claim date: {_dashboardControlModel.BotStatistics.LastCreditClaimDateTime:g}";
        set
        {
            _dashboardControlModel.BotStatistics.LastCreditClaimDateTime = DateTime.Parse(value);
            _botStatisticsManager.SaveStatistics(_dashboardControlModel.BotStatistics);
            OnPropertyChanged();
        }
    }

    public string ClaimButtonText
    {
        get => _dashboardControlModel.ClaimButtonText;
        set
        {
            _dashboardControlModel.ClaimButtonText = value;
            OnPropertyChanged();
        }
    }

    public string BotOperationStatus
    {
        get => _dashboardControlModel.BotOperationStatus;
        set
        {
            _dashboardControlModel.BotOperationStatus = value;
            OnPropertyChanged();
        }
    }

    #endregion  

}