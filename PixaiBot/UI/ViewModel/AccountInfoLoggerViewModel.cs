using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Notification.Wpf;
using PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy;
using PixaiBot.Business_Logic.Extension;
using PixaiBot.Business_Logic.Logging;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;
using PixaiBot.UI.Models;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace PixaiBot.UI.ViewModel;

internal class AccountInfoLoggerViewModel : BaseViewModel
{
    #region Commands

    public ICommand StartLoggingCommand { get; }

    #endregion

    #region Constructor

    public AccountInfoLoggerViewModel(IAccountInfoLogger accountInfoLogger,
        IAccountsManager accountsManager, IConfigManager configManager,
        IToastNotificationSender toastNotificationSender, ILogger logger)
    {
        StartLoggingCommand = new RelayCommand(_ => StartLogging());

        _accountInfoLogger = accountInfoLogger;
        _accountsManager = accountsManager;
        _configManager = configManager;
        _logger = logger;
        _toastNotificationSender = toastNotificationSender;
        _accountInfoLoggerModel = new AccountInfoLoggerModel();

        IsRunning = false;
        OperationStatus = "Idle.";
        LogButtonText = "Start Logging";
    }

    #endregion

    #region Methods

    private async void StartLogging()
    {
        _logger.Log("Account info logging process started", _logger.ApplicationLogFilePath);

        if (IsRunning)
        {
            StopLogging();
            return;
        }

        var config = _configManager.GetConfig();
        _tokenSource = new CancellationTokenSource();

        IsRunning = true;
        OperationStatus = "Running...";
        LogButtonText = "Stop";

        IDriverCreationStrategy driverCreationStrategy = config.HeadlessBrowser
            ? new HeadlessDriverCreationStrategy()
            : new HiddenDriverCreationStrategy();

        _accountInfoLogger.ClearStringBuilderContent();
        var result = string.Empty;
        if (config.MultiThreading)
        {
            var accounts = _accountsManager.GetAllAccounts().SplitList(config.NumberOfThreads);
            _logger.Log("Multi-threading enabled\nCreating a tasks to do", _logger.ApplicationLogFilePath);

            var tasks = accounts.Select(account => Task.Run(() =>
                _accountInfoLogger.StartLoggingAccountsInfo(account, driverCreationStrategy, _accountInfoLoggerModel, _tokenSource.Token)));

            await Task.WhenAll(tasks);

            var completedTask = await Task.WhenAny(tasks);

            result = await completedTask;
        }
        else
        {
            _logger.Log("Creating a task to do", _logger.ApplicationLogFilePath);

            result = await Task.Run(() =>
                _accountInfoLogger.StartLoggingAccountsInfo(_accountsManager.GetAllAccounts(),
                    driverCreationStrategy, _accountInfoLoggerModel, _tokenSource.Token));
        }

        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
            FileName = "AccountsInfo.txt"
        };

        if (saveFileDialog.ShowDialog() == true) await File.WriteAllTextAsync(saveFileDialog.FileName, result);

        StopLogging();
    }

    private void StopLogging()
    {
        _logger.Log("Account info logging process Ended", _logger.ApplicationLogFilePath);
        IsRunning = false;
        OperationStatus = "Idle.";
        LogButtonText = "Start Logging";
        _tokenSource.Cancel();
    }

    #endregion

    #region Fields

    private readonly IAccountInfoLogger _accountInfoLogger;


    private readonly IAccountsManager _accountsManager;

    private readonly IConfigManager _configManager;

    private readonly IToastNotificationSender _toastNotificationSender;

    private readonly ILogger _logger;

    private readonly AccountInfoLoggerModel _accountInfoLoggerModel;

    private CancellationTokenSource _tokenSource;

    public bool IsRunning
    {
        get => _accountInfoLoggerModel.IsRunning;

        set
        {
            _accountInfoLoggerModel.IsRunning = value;
            OnPropertyChanged();
        }
    }

    public bool ShouldLogAccountId
    {
        get => _accountInfoLoggerModel.ShouldLogAccountId;

        set
        {
            _accountInfoLoggerModel.ShouldLogAccountId = value;
            OnPropertyChanged();
        }
    }

    public bool ShouldLogAccountUsername
    {
        get => _accountInfoLoggerModel.ShouldLogAccountUsername;

        set
        {
            _accountInfoLoggerModel.ShouldLogAccountUsername = value;
            OnPropertyChanged();
        }
    }

    public bool ShouldLogEmailVerificationStatus
    {
        get => _accountInfoLoggerModel.ShouldLogEmailVerificationStatus;

        set
        {
            _accountInfoLoggerModel.ShouldLogEmailVerificationStatus = value;
            OnPropertyChanged();
        }
    }

    public bool ShouldLogFollowersCount
    {
        get => _accountInfoLoggerModel.ShouldLogFollowersCount;

        set
        {
            _accountInfoLoggerModel.ShouldLogFollowersCount = value;
            OnPropertyChanged();
        }
    }

    public bool ShouldLogFollowingCount
    {
        get => _accountInfoLoggerModel.ShouldLogFollowingCount;
        set
        {
            _accountInfoLoggerModel.ShouldLogFollowingCount = value;
            OnPropertyChanged();
        }
    }

    public bool ShouldLogAccountCredits
    {
        get => _accountInfoLoggerModel.ShouldLogAccountCredits;
        set
        {
            _accountInfoLoggerModel.ShouldLogAccountCredits = value;
            OnPropertyChanged();
        }
    }

    public string OperationStatus
    {
        get => _accountInfoLoggerModel.OperationStatus;
        set
        {
            _accountInfoLoggerModel.OperationStatus = value;
            OnPropertyChanged();
        }
    }

    public string LogButtonText
    {
        get => _accountInfoLoggerModel.LogButtonText;

        set
        {
            _accountInfoLoggerModel.LogButtonText = value;
            OnPropertyChanged();
        }
    }

    #endregion
}