using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using Notification.Wpf;
using PixaiBot.Bussines_Logic.Data_Handling;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;
using PixaiBot.UI.Base;

namespace PixaiBot.UI.ViewModel
{

    internal class LogAccountInfoControlViewModel : BaseViewModel
    {

        #region Commands

        public ICommand StartLoggingCommand { get; }

        #endregion

        #region Constructor 
        public LogAccountInfoControlViewModel(IAccountsInfoLogger accountsInfoLogger, ITcpServerConnector tcpServerConnector,
            IAccountsManager accountsManager, IConfigManager configManager, IToastNotificationSender toastNotificationSender)
        {
            _accountsInfoLogger = accountsInfoLogger;
            _tcpServerConnector = tcpServerConnector;
            _accountsManager = accountsManager;
            _configManager = configManager;
            _toastNotificationSender = toastNotificationSender;
            StartLoggingCommand = new RelayCommand((obj) => StartLogging());

            _accountInfoLoggerModel = new AccountInfoLoggerModel();

            IsRunning = false;
            Status = "Idle.";
            LogButtonText = "Start Logging";
        }
        #endregion

        #region Methods

        public async void StartLogging()
        {
            if (IsRunning)
            {
                StopLogging();
                return;
            }

            var config = _configManager.GetConfig();
            _tokenSource = new CancellationTokenSource();

            IsRunning = true;
            Status = "Running...";
            LogButtonText = "Stop";
            var result = string.Empty;
            if (config.MultiThreading)
            {
                var accounts = _accountsManager.GetAllAccounts().SplitList(config.NumberOfThreads);

                var tasks = accounts.Select(account => Task.Run(() => _accountsInfoLogger.StartLoggingAccountsInfo(account, _accountInfoLoggerModel, _tokenSource.Token)));

                await Task.WhenAll(tasks);

                var completedTasks = tasks.Where(t => t.IsCompletedSuccessfully);

                result = string.Join(Environment.NewLine, completedTasks.Select(t => t.Result));
            }
            else
            { 
                result = await Task .Run(() => _accountsInfoLogger.StartLoggingAccountsInfo(_accountsManager.GetAllAccounts(), _accountInfoLoggerModel, _tokenSource.Token));
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                FileName = "AccountsInfo.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllTextAsync(saveFileDialog.FileName, result);
            }

            StopLogging();
        }

        private void StopLogging()
        {
            IsRunning = false;
            Status = "Idle.";
            LogButtonText = "Start Logging";
            _tokenSource.Cancel();
        }

        #endregion
        #region Fields

        private readonly IAccountsInfoLogger _accountsInfoLogger;

        private readonly ITcpServerConnector _tcpServerConnector;

        private readonly IAccountsManager _accountsManager;

        private readonly IConfigManager _configManager;

        private readonly IToastNotificationSender _toastNotificationSender;

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

        public string Status
        {
            get => _accountInfoLoggerModel.Status;
            set
            {
                _accountInfoLoggerModel.Status = value;
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

}
