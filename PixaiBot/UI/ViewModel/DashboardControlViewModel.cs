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
using PixaiBot.Bussines_Logic;
using Brush = System.Drawing.Brush;

namespace PixaiBot.UI.ViewModel;

public class DashboardControlViewModel : BaseViewModel
{
    public ICommand ClaimCreditsCommand { get; }

    public DashboardControlViewModel(ICreditClaimer creditClaimer, IAccountsManager accountsManager,
        IBotStatisticsManager botStatisticsManager, ILogger logger, IConfigManager configManager,
        IToastNotificationSender toastNotificationSender,ITcpServerConnector tcpServerConnector)
    {
        _configManager = configManager;

        _logger = logger;

        _botStatisticsManager = botStatisticsManager;

        _creditClaimer = creditClaimer;

        _tcpServerConnector = tcpServerConnector;

        _accountsManager = accountsManager;

        _toastNotificationSender = toastNotificationSender;

        ClaimCreditsCommand = new RelayCommand((obj) => ClaimCreditsInNewThread());

        _creditClaimer.CreditClaimed += CreditClaimed;

        _botStatisticsManager.StatisticsChanged += StatisticsRefreshed;

        StatisticsRefreshed(null, EventArgs.Empty);

        if (_configManager.ShouldAutoClaimCredits) StartCreditsAutoClaim();
        
       _regularBrush =  new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8964ff"));

       _redBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e74c3c"));

       ClaimCreditButtonBrushColor = _regularBrush;
        
        ClaimCreditButtonText = "Claim Credits";
    }

    private readonly SolidColorBrush _redBrush;

    private readonly SolidColorBrush _regularBrush; 

    private readonly ITcpServerConnector _tcpServerConnector;

    private CancellationTokenSource _cancellationTokenSource;

    private const int CreditClaimerInterval = 24;

    private readonly IAccountsManager _accountsManager;

    private readonly ICreditClaimer _creditClaimer;

    private readonly IBotStatisticsManager _botStatisticsManager;

    private readonly IConfigManager _configManager;

    private readonly ILogger _logger;

    private readonly IToastNotificationSender _toastNotificationSender;

    private string? _accountCount;

    private string _creditClaimerInfo;

    private bool _isClaimingCredits;

    public string CreditClaimerInfo
    {
        get => _creditClaimerInfo;
        set
        {
            _creditClaimerInfo = value;
            OnPropertyChanged();
        }
    }


    private SolidColorBrush _claimCreditButtonButtonColor;

    public SolidColorBrush ClaimCreditButtonBrushColor
    {
        get => _claimCreditButtonButtonColor;
        set
        {
            _claimCreditButtonButtonColor = value;
            OnPropertyChanged();
        }
    }


    private string _claimCreditButtonText;

    public string ClaimCreditButtonText
    {
        get => _claimCreditButtonText;
        set
        {
            _claimCreditButtonText = value;
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
        _tcpServerConnector.SendMessage("gStatistics refreshed");

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
        if (_isClaimingCredits)
        {
            _tcpServerConnector.SendMessage("rCredits Claiming Process Stopped");

            ClaimCreditButtonText = "Claim Credits"; 
            
            _isClaimingCredits = false;
            
            ClaimCreditButtonBrushColor = _regularBrush;
            
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
           
            return;
        }

        _cancellationTokenSource = new CancellationTokenSource();

        ClaimCreditButtonText = "Stop Claiming";

        ClaimCreditButtonBrushColor = _redBrush;

        _tcpServerConnector.SendMessage("cCredits Claiming Process Started");


        _isClaimingCredits = true;

        if (_configManager.ShouldSendToastNotifications)
            _creditClaimer.ClaimCreditsForAllAccounts(_accountsManager.GetAllAccounts(), _cancellationTokenSource.Token, _toastNotificationSender);
        else
            _creditClaimer.ClaimCreditsForAllAccounts(_accountsManager.GetAllAccounts(), _cancellationTokenSource.Token);

        _logger.Log("Credits claimed", _logger.ApplicationLogFilePath);

        CreditClaimerInfo = "Credits Claimed.";

        LastCreditClaimDateTime = DateTime.Now.ToString();

        ClaimCreditButtonText = "Claim Credits";

        _tcpServerConnector.SendMessage("gCredits Claimed");


        ClaimCreditButtonBrushColor = _regularBrush;

    }




}