using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using Notification.Wpf;
using PixaiBot.Business_Logic.Data_Management;
using PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;
using PixaiBot.UI.Models;

namespace PixaiBot.UI.ViewModel;

internal class AccountCreatorViewModel : BaseViewModel
{
    #region Constructor

    public AccountCreatorViewModel(IProxyManager proxyManager, ILogger logger, IAccountsManager accountsManager,
        IToastNotificationSender toastNotificationSender, IAccountCreator accountCreator,
        IConfigManager configManager)
    {
        AddProxyCommand = new RelayCommand(_ => AddProxy());
        StartAccountCreationCommand = new RelayCommand(_ => StartAccountCreation());

        _accountCreatorModel = new AccountCreatorModel();
        _configManager = configManager;
        _accountCreator = accountCreator;
        _toastNotificationSender = toastNotificationSender;
        _logger = logger;
        _accountsManager = accountsManager;
        _proxyManager = proxyManager;

        ProxyFilePath = "Select Proxy File";
        OperationStatus = "Idle.";
        AccountsCreatorButtonText = "Start Account Creation";

        _accountCreator.AccountCreated += OnAccountCreated;
        _accountCreator.ErrorOccurred += OnErrorOccurred;
    }

    #endregion

    #region Commands

    public ICommand AddProxyCommand { get; }

    public ICommand StartAccountCreationCommand { get; }

    #endregion

    #region Methods

    private void AddProxy()
    {
        _logger.Log("Adding proxy", _logger.ApplicationLogFilePath);
        var dialog = new OpenFileDialog
        {
            Title = "Select File:",
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        };

        var result = dialog.ShowDialog();

        if (result == false) return;

        ProxyFilePath = dialog.FileName;

        _proxyManager.ReadProxyFile(ProxyFilePath);
    }

    private void OnAccountCreated(object? sender, UserAccount e)
    {
        _accountsManager.AddAccount(e);
        _logger.Log("Account created, adding account to account list", _logger.ApplicationLogFilePath);

        if (_configManager.GetConfig().ToastNotifications)
            _toastNotificationSender.SendNotification("PixaiBot", "Account Created", NotificationType.Success);
    }

    private void OnErrorOccurred(object? sender, string e)
    {
        if (_configManager.GetConfig().ToastNotifications)
            _toastNotificationSender.SendNotification("PixaiBot", e, NotificationType.Error);
    }

    private async void StartAccountCreation()
    {
        _logger.Log("Account creation process started", _logger.ApplicationLogFilePath);

        if (IsRunning)
        {
            StopCreating();
            return;
        }

        _tokenSource = new CancellationTokenSource();

        IsRunning = true;
        OperationStatus = "Running...";
        AccountsCreatorButtonText = "Stop Account Creation";

        _logger.Log("Creating a task to do", _logger.ApplicationLogFilePath);

        if (!int.TryParse(AccountAmount, out var amount)) return;

        IDriverCreationStrategy driverCreationStrategy = ShouldUseProxy
            ? new ProxyDriverCreationStrategy(_proxyManager)
            : new HeadlessDriverCreationStrategy();

        if (Configuration.IsDevEnv) driverCreationStrategy = new DebugDriverCreationStrategy();

        if (_configManager.GetConfig().ToastNotifications)
            _toastNotificationSender.SendNotification("PixaiBot", "Account creation process started",
                NotificationType.Information);

        await Task.Run(() =>
        {
            // Determine the interval based on proxy usage
            var interval = ShouldUseProxy ? TimeSpan.Zero : TimeSpan.FromMinutes(5);

            // Pixai limits the amount of accounts that can be created from the same IP address.
            // if the user uses a proxy, the interval may be zero because a different proxy will be selected for each account
            // if the user does not use a proxy, the interval will be 5 minutes to avoid being blocked by rate limiter.
            _accountCreator.CreateAccounts(amount, TempMailApiKey, ShouldVerifyEmail, driverCreationStrategy,
                interval, _tokenSource.Token);
        });

        StopCreating();
    }

    private void StopCreating()
    {
        IsRunning = false;
        OperationStatus = "Idle.";
        if (_configManager.GetConfig().ToastNotifications)
            _toastNotificationSender.SendNotification("PixaiBot", "Account creation process ended",
                NotificationType.Information);
        AccountsCreatorButtonText = "Start Account Creation";
        _tokenSource.Cancel();
        _logger.Log("Account creation process ended", _logger.ApplicationLogFilePath);
    }

    #endregion

    #region Fields

    private readonly IProxyManager _proxyManager;

    private readonly ILogger _logger;

    private readonly IAccountsManager _accountsManager;

    private readonly IAccountCreator _accountCreator;

    private readonly IToastNotificationSender _toastNotificationSender;

    private readonly IConfigManager _configManager;

    private readonly AccountCreatorModel _accountCreatorModel;

    private CancellationTokenSource _tokenSource;

    public bool IsRunning
    {
        get => _accountCreatorModel.IsRunning;
        set
        {
            _accountCreatorModel.IsRunning = value;
            OnPropertyChanged();
        }
    }

    public bool ShouldUseProxy
    {
        get => _accountCreatorModel.ShouldUseProxy;
        set
        {
            _accountCreatorModel.ShouldUseProxy = value;
            OnPropertyChanged();
        }
    }

    public bool ShouldVerifyEmail
    {
        get => _accountCreatorModel.ShouldVerifyEmail;
        set
        {
            _accountCreatorModel.ShouldVerifyEmail = value;
            OnPropertyChanged();
        }
    }

    public string TempMailApiKey
    {
        get => _accountCreatorModel.TempMailApiKey;
        set
        {
            _accountCreatorModel.TempMailApiKey = value;
            OnPropertyChanged();
        }
    }

    public string ProxyFilePath
    {
        get => _accountCreatorModel.ProxyFilePath;
        set
        {
            _accountCreatorModel.ProxyFilePath = value;
            OnPropertyChanged();
        }
    }

    public string AccountAmount
    {
        get => _accountCreatorModel.AccountsAmmount;
        set
        {
            _accountCreatorModel.AccountsAmmount = value;
            OnPropertyChanged();
        }
    }

    public string OperationStatus
    {
        get => _accountCreatorModel.OperationStatus;
        set
        {
            _accountCreatorModel.OperationStatus = value;
            OnPropertyChanged();
        }
    }

    public string AccountsCreatorButtonText
    {
        get => _accountCreatorModel.AccountsCreatorButtonText;
        set
        {
            _accountCreatorModel.AccountsCreatorButtonText = value;
            OnPropertyChanged();
        }
    }

    #endregion
}