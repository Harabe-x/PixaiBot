using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;
using PixaiBot.UI.Base;
using PixaiBot.UI.View;

namespace PixaiBot.UI.ViewModel
{
    internal class AccountListControlViewModel : BaseViewModel
    {
        public ICommand EditAccountCommand { get; }

        public ICommand RemoveAccountCommand { get; }

        public AccountListControlViewModel(ITcpServerConnector tcpServerConnector,IAccountsManager accountsManager,ILogger logger,IDialogService dialogService,IDataValidator DataValidator)
        {
            _dataValidator = DataValidator;
            _accountsManager = accountsManager;
            _dialogService = dialogService;
            _logger = logger;
            _tcpServerConnector = tcpServerConnector; 
            RemoveAccountCommand = new RelayCommand((obj) => RemoveAccount());
            EditAccountCommand = new RelayCommand((obj) => EditAccount());
            UserAccounts = new ObservableCollection<UserAccount>(_accountsManager.GetAllAccounts());
            _accountsManager.AccountsListChanged += AccountsManagerOnAccountsListChanged;
            
        }

        private readonly ITcpServerConnector _tcpServerConnector; 

        private readonly ILogger _logger;

        private readonly IDialogService _dialogService;

        private readonly IDataValidator _dataValidator;

        private readonly IAccountsManager _accountsManager;

        private UserAccount _selectedAccount;

        public UserAccount SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged();
            }
        }
        
        private ObservableCollection<UserAccount> _userAccounts;

        public ObservableCollection<UserAccount> UserAccounts
        {
            get => _userAccounts;
            set
            {
                _userAccounts = value;
                OnPropertyChanged();
            }
        }

        private void RemoveAccount()
        {
   
            if (SelectedAccount == null) return;


            _logger.Log("Remove account command called", _logger.ApplicationLogFilePath);
            _tcpServerConnector.SendMessage("gAccount Removed");
            _accountsManager.RemoveAccount(SelectedAccount);
        }

        private void EditAccount()
        {
            _logger.Log("Edit account command called",_logger.ApplicationLogFilePath);
            
            if (SelectedAccount == null) return;

            if (string.IsNullOrEmpty(SelectedAccount.Email) || string.IsNullOrEmpty(SelectedAccount.Password)) return;
            _tcpServerConnector.SendMessage("mUser editing  account info");

            _dialogService.ShowDialog(new EditAccountWindowView(),new EditAccountWindowViewModel(_accountsManager,_logger,SelectedAccount,_dataValidator),true);


        }

        private void AccountsManagerOnAccountsListChanged(object? sender, EventArgs e)
        {
            UserAccounts = new ObservableCollection<UserAccount>(_accountsManager.GetAllAccounts());
            _logger.Log("Accounts list refreshed",_logger.ApplicationLogFilePath);
            _tcpServerConnector.SendMessage("c Accounts list updated");

        }

    }
}
