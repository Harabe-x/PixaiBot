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

            _accountInfoLoggerrSettings = new AccountInfoLoggerSettings();
        }
        #endregion
        #region Methods

        public void StartLogging()
        {
            if (_configManager.GetConfig().ToastNotifications)
                _toastNotificationSender.SendNotification("PixaiBot", "Account Info Logging Started", NotificationType.Information);

            if (_isRunning)
            {
                _cancellationTokenSource.Cancel();
                _isRunning = false;
            }
            else
            {
                _cancellationTokenSource = new CancellationTokenSource();
            }

            var accounts = _accountsManager.GetAllAccounts();

            _isRunning = true;

            _accountsInfoLogger.StartLoggingAccountsInfo(accounts, _accountInfoLoggerrSettings,
                _cancellationTokenSource.Token);
        }

        #endregion
        #region Fields

        private readonly IAccountsInfoLogger _accountsInfoLogger;

        private readonly ITcpServerConnector _tcpServerConnector;

        private readonly IAccountsManager _accountsManager;

        private readonly IConfigManager _configManager;

        private readonly IToastNotificationSender _toastNotificationSender;

        public readonly AccountInfoLoggerSettings _accountInfoLoggerrSettings;

        private CancellationTokenSource _cancellationTokenSource;

        private bool _isRunning;


        public bool ShouldLogAccountId
        {
            get => _accountInfoLoggerrSettings.ShouldLogAccountId;

            set
            {
                _accountInfoLoggerrSettings.ShouldLogAccountId = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldLogAccountUsername
        {
            get => _accountInfoLoggerrSettings.ShouldLogAccountUsername;

            set
            {
                _accountInfoLoggerrSettings.ShouldLogAccountUsername = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldLogEmailVerificationStatus
        {
            get => _accountInfoLoggerrSettings.ShouldLogEmailVerificationStatus;

            set
            {
                _accountInfoLoggerrSettings.ShouldLogEmailVerificationStatus = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldLogFollowersCount
        {
            get => _accountInfoLoggerrSettings.ShouldLogFollowersCount;

            set
            {
                _accountInfoLoggerrSettings.ShouldLogFollowersCount = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldLogFollowingCount
        {
            get => _accountInfoLoggerrSettings.ShouldLogFollowingCount;
            set
            {
                _accountInfoLoggerrSettings.ShouldLogFollowingCount = value;
                OnPropertyChanged();
            }
        }
        #endregion  



    }

}
