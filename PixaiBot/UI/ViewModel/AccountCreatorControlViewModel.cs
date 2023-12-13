using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Notification.Wpf;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;
using PixaiBot.UI.Base;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace PixaiBot.UI.ViewModel
{
    internal class AccountCreatorControlViewModel : BaseViewModel
    {

        public ICommand AddProxyCommand { get; }

        public ICommand StartAccountCreationCommand { get; }


        public AccountCreatorControlViewModel(ITcpServerConnector tcpServerConnector,IProxyManager proxyManager,ILogger logger,IDialogService dialogService,IAccountsManager accountsManager,IToastNotificationSender toastNotificationSender,IAccountCreator accountCreator,IConfigManager configManager)
        {
            _configManager = configManager;
            _accountCreator = accountCreator;
            _toastNotificationSender = toastNotificationSender;
            _logger = logger;
            _tcpServerConnector = tcpServerConnector; 
            _dialogService = dialogService;
            _accountsManager = accountsManager;
            _proxyManager = proxyManager;
            AddProxyCommand = new RelayCommand((obj) => AddProxy());
            StartAccountCreationCommand = new RelayCommand((obj) => StartAccountCreation());
            ProxyFilePath = "Select Proxy File";
            _accountCreator.AccountCreated += OnAccountCreated;
            _accountCreator.ErrorOccurred += OnErrorOccurred;
        }

       


        private readonly IProxyManager _proxyManager;

        private readonly ILogger _logger;

        private readonly IDialogService _dialogService;

        private readonly IAccountsManager _accountsManager;

        private readonly IAccountCreator _accountCreator;

        private readonly IToastNotificationSender _toastNotificationSender;

        private readonly IConfigManager _configManager;

        private readonly ITcpServerConnector _tcpServerConnector;


        private string _accountAmount;

        public string AccountAmount
        {
            get => _accountAmount;
            set
            {
                _accountAmount = value;
                OnPropertyChanged();
            }
        }

        private string _tempMailApiKey;

        public string TempMailApiKey
        {
            get => _tempMailApiKey;

            set
            {
                _tempMailApiKey = value;
                OnPropertyChanged();
            }
        }

        private bool _shouldVerifyEmail;

        public bool ShouldVerifyEmail
        {
            get => _shouldVerifyEmail;

            set
            {
                _shouldVerifyEmail = value;
                OnPropertyChanged();
            }
        }

        private bool _shouldUseProxy;

        public bool ShouldUseProxy
        {
            get => _shouldUseProxy;
            set
            {
                _shouldUseProxy = value;
                OnPropertyChanged();
            }
        }

        private string _proxyFilePath;

        public string ProxyFilePath
        {
            get => _proxyFilePath;
            set
            {
                _proxyFilePath = value;
                OnPropertyChanged();
            }
        }

        private void AddProxy()
        {

            _tcpServerConnector.SendMessage("mUser Adding proxy ");

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
            _tcpServerConnector.SendMessage($"mAccount Created: {e.Email}:{e.Password}");
            _accountsManager.AddAccount(e);
            if (_configManager.ShouldSendToastNotifications) { _toastNotificationSender.SendNotification("PixaiBot", $"Account Created", NotificationType.Success); }

        }
        private void OnErrorOccurred(object? sender, string e)
        {
            _tcpServerConnector.SendMessage($"rError Occurred: {e}");
            if(_configManager.ShouldSendToastNotifications) { _toastNotificationSender.SendNotification("PixaiBot",e,NotificationType.Error); }
        }

        private void StartAccountCreation()
        {
            _tcpServerConnector.SendMessage("mUser Starting account creation");

            if (!int.TryParse(AccountAmount, out var amount) || amount > 125) return;

            var task = new Task( () => { _accountCreator.CreateAccounts(amount, TempMailApiKey, ShouldUseProxy, ShouldVerifyEmail); });
            
            task.Start();
        }
    }
}
