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

            AccountInfoLoggerModel = new AccountInfoLoggerModel();
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

            _accountsInfoLogger.StartLoggingAccountsInfo(accounts, AccountInfoLoggerModel,
                _cancellationTokenSource.Token);
        }

        #endregion
        #region Fields

        private readonly IAccountsInfoLogger _accountsInfoLogger;

        private readonly ITcpServerConnector _tcpServerConnector;

        private readonly IAccountsManager _accountsManager;

        private readonly IConfigManager _configManager;

        private readonly IToastNotificationSender _toastNotificationSender;

        public readonly AccountInfoLoggerModel AccountInfoLoggerModel;

        private CancellationTokenSource _cancellationTokenSource;

        private bool _isRunning;


        public bool ShouldLogAccountId
        {
            get => AccountInfoLoggerModel.ShouldLogAccountId;

            set
            {
                AccountInfoLoggerModel.ShouldLogAccountId = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldLogAccountUsername
        {
            get => AccountInfoLoggerModel.ShouldLogAccountUsername;

            set
            {
                AccountInfoLoggerModel.ShouldLogAccountUsername = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldLogEmailVerificationStatus
        {
            get => AccountInfoLoggerModel.ShouldLogEmailVerificationStatus;

            set
            {
                AccountInfoLoggerModel.ShouldLogEmailVerificationStatus = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldLogFollowersCount
        {
            get => AccountInfoLoggerModel.ShouldLogFollowersCount;

            set
            {
                AccountInfoLoggerModel.ShouldLogFollowersCount = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldLogFollowingCount
        {
            get => AccountInfoLoggerModel.ShouldLogFollowingCount;
            set
            {
                AccountInfoLoggerModel.ShouldLogFollowingCount = value;
                OnPropertyChanged();
            }
        }
        #endregion  



    }

}
