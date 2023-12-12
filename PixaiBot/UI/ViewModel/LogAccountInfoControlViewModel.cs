using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Notification.Wpf;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;

namespace PixaiBot.UI.ViewModel
{

    internal class LogAccountInfoControlViewModel : BaseViewModel
    {

        public ICommand StartLoggingCommand { get; }

        public LogAccountInfoControlViewModel(IAccountsInfoLogger accountsInfoLogger, ITcpServerConnector tcpServerConnector,
            IAccountsManager accountsManager, IConfigManager configManager, IToastNotificationSender toastNotificationSender)
        {
            _accountsInfoLogger = accountsInfoLogger;
            _tcpServerConnector = tcpServerConnector;
            _accountsManager = accountsManager;
            _configManager = configManager;
            _toastNotificationSender = toastNotificationSender;
            StartLoggingCommand = new RelayCommand((obj) => StartLogging());
        }

        private readonly IAccountsInfoLogger _accountsInfoLogger;

        private readonly ITcpServerConnector _tcpServerConnector;

        private readonly IAccountsManager _accountsManager;

        private readonly IConfigManager _configManager;

        private readonly IToastNotificationSender _toastNotificationSender;

        private CancellationTokenSource _cancellationTokenSource;

        private bool _isRunning;


        public void StartLogging()
        {
            if (_configManager.ShouldSendToastNotifications)
                _toastNotificationSender.SendNotification("PixaiBot", "Account Info Logging Started", NotificationType.Information);


            _cancellationTokenSource = new CancellationTokenSource();

            var accounts = _accountsManager.GetAllAccounts();


            if (_isRunning)
            {
                _cancellationTokenSource.Cancel();
            }

            _isRunning = true;

            _cancellationTokenSource = new CancellationTokenSource();

            _accountsInfoLogger.StartLoggingAccountsInfo(accounts,null/*dummy for testing purposes*/, _cancellationTokenSource.Token);

            _isRunning = false;
        }


    }

}
