using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Notification.Wpf;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;
using PixaiBot.UI.Base;
using PixaiBot.UI.Models;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace PixaiBot.UI.ViewModel;

internal class AccountCreatorViewModel : BaseViewModel
{
    #region Commands

    public ICommand AddProxyCommand { get; }

    public ICommand StartAccountCreationCommand { get; }

    #endregion

    #region Constructor

    public AccountCreatorViewModel(IProxyManager proxyManager, ILogger logger, IAccountsManager accountsManager,
        IToastNotificationSender toastNotificationSender, IAccountCreator accountCreator,
        IConfigManager configManager)
    {
        _accountCreatorModel = new AccountCreatorModel();
        _configManager = configManager;
        _accountCreator = accountCreator;
        _toastNotificationSender = toastNotificationSender;
        _logger = logger;
        _accountsManager = accountsManager;
        _proxyManager = proxyManager;

        AddProxyCommand = new RelayCommand((obj) => AddProxy());
        StartAccountCreationCommand = new RelayCommand((obj) => StartAccountCreation());

        ProxyFilePath = "Select Proxy File";
        OperationStatus = "Idle.";
        AccountsCreatorButtonText = "Start Account Creation";

        _accountCreator.AccountCreated += OnAccountCreated;
        _accountCreator.ErrorOccurred += OnErrorOccurred;
    }

    #endregion

    #region Methods

    private void AddProxy()
    {
        var dialog = new OpenFileDialog()
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
        if (_configManager.GetConfig().ToastNotifications)
            _toastNotificationSender.SendNotification("PixaiBot", $"Account Created", NotificationType.Success);
    }

    private void OnErrorOccurred(object? sender, string e)
    {
        if (_configManager.GetConfig().ToastNotifications)
            _toastNotificationSender.SendNotification("PixaiBot", e, NotificationType.Error);
    }

    private async void StartAccountCreation()
    {
        if (IsRunning)
        {
            StopCreating();
            return;
        }

        _tokenSource = new CancellationTokenSource();

        IsRunning = true;
        OperationStatus = "Running...";
        AccountsCreatorButtonText = "Stop Account Creation";


        if (!int.TryParse(AccountAmount, out var amount)) return;

        await Task.Run(() =>
        {
            _accountCreator.CreateAccounts(amount, TempMailApiKey, ShouldUseProxy, ShouldVerifyEmail,
                _tokenSource.Token);
        });

        StopCreating();
    }

    private void StopCreating()
    {
        IsRunning = false;
        OperationStatus = "Idle.";
        AccountsCreatorButtonText = "Start Account Creation";
        _tokenSource.Cancel();
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