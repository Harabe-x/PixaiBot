using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Notification.Wpf;
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

        public void StartLogging()
        {
            if (IsRunning)
            {
                IsRunning = false;
                Status = "Idle.";
                LogButtonText = "Start Logging";
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                return;
            }
            else
            {
                IsRunning = true;
                Status = "Running...";
                LogButtonText = "Stop";
                _cancellationTokenSource = new CancellationTokenSource();
            }

            Task.Run(() =>
            {

                // IsRunning = false;
                //Status = "Idle.";
                //LogButtonText = "Start Logging";
                //_cancellationTokenSource.Cancel();
                //_cancellationTokenSource.Dispose();
            });
        }

        #endregion
        #region Fields

        private readonly IAccountsInfoLogger _accountsInfoLogger;

        private readonly ITcpServerConnector _tcpServerConnector;

        private readonly IAccountsManager _accountsManager;

        private readonly IConfigManager _configManager;

        private readonly IToastNotificationSender _toastNotificationSender;

        private readonly AccountInfoLoggerModel _accountInfoLoggerModel;

        private CancellationTokenSource _cancellationTokenSource;

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
