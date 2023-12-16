using PixaiBot.Data.Interfaces;
using System.Windows.Threading;
using System.Threading.Tasks;
using PixaiBot.Data.Models;
using System.Windows.Input;
using PixaiBot.UI.Base;
using System.Linq;
using System;
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
        _creditClaimer.CreditClaimed += UpdateBotOperationStatus;

        ClaimButtonText = "Start Claiming";
        BotOperationStatus = "idle.";
    }

    

    #region Methods

    public void ClaimCredits()
    {
        if (IsRunning)
        {
            IsRunning = false;
            ClaimButtonText = "Start Claiming";
            BotOperationStatus = "idle.";
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
            if(_configManager.GetConfig().ToastNotifications) _creditClaimer.ClaimCreditsForAllAccounts(_accountsManager.GetAllAccounts(),_tokenSource.Token,_notificationSender);
            else _creditClaimer.ClaimCreditsForAllAccounts(_accountsManager.GetAllAccounts(),_tokenSource.Token);
            IsRunning = false;
            ClaimButtonText = "Start Claiming";
            BotOperationStatus = "idle.";
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        });
    }
    private void UpdateBotOperationStatus(object? sender, UserAccount e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            BotOperationStatus = $"Claiming credits for {e.Email}";
        });
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
        get => _dashboardControlModel.BotVersion;
        set
        {
             _dashboardControlModel.BotVersion = value;
             OnPropertyChanged();
        }
    }

    public string AccountsCount
    {
        get => _dashboardControlModel.AccountsCount;
        set
        {
            _dashboardControlModel.AccountsCount = value;
            OnPropertyChanged();
        }
    }

    public string LastCreditClaimDate
    {
        get => _dashboardControlModel.LastCreditClaimDateTime;
        set
        {
            _dashboardControlModel.LastCreditClaimDateTime = value;
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