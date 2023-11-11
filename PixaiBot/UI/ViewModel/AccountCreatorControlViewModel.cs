﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace PixaiBot.UI.ViewModel
{
    internal class AccountCreatorControlViewModel : BaseViewModel
    {

        public ICommand AddProxyCommand { get; }

        public ICommand StartAccountCreationCommand { get; }


        public AccountCreatorControlViewModel(IProxyManager proxyManager,ILogger logger,IDialogService dialogService,IAccountsManager accountsManager,ITempMailApiManager tempMaiLApiManager)
        {
            _logger = logger;
            _dialogService = dialogService;
            _accountsManager = accountsManager;
            _tempMailApiManager = tempMaiLApiManager;
            _proxyManager = proxyManager;
            AddProxyCommand = new RelayCommand((obj) => AddProxy());
            StartAccountCreationCommand = new RelayCommand((obj) => StartAccountCreation());
            ProxyFilePath = "Select Proxy File";
            _tempMailApiManager.RequestFailed += TempMailApiManagerOnRequestFailed;
        }

        

        private readonly IProxyManager _proxyManager;

        private readonly ILogger _logger;

        private readonly IDialogService _dialogService;

        private readonly IAccountsManager _accountsManager;

        private readonly ITempMailApiManager _tempMailApiManager;

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
            MessageBox.Show(_tempMailApiManager.GetEmail(TempMailApiKey));;
            //var dialog = new OpenFileDialog()
            //{
            //    Title = "Select File:",
            //    Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
            //    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            //};

            //var result = dialog.ShowDialog();

            //if (result == false) return;

            //ProxyFilePath = dialog.FileName;

            //_proxyManager.ReadProxyFile(ProxyFilePath);

        }

        private void TempMailApiManagerOnRequestFailed(object? sender, string e)
        {
            MessageBox.Show(" !!!!!!!!!  !!!!!!!!!! !!!!!!!!!!! !!!!!! !!!!! !!!!! !!!!!!! ");
        }

        private void StartAccountCreation()
        {

        }
    }
}
