using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;
using PixaiBot.UI.Base;

namespace PixaiBot.UI.ViewModel
{
    internal class EditAccountWindowViewModel : BaseViewModel , IWindowHelper
    {
        public ICommand CloseWindowCommand { get; }

        public ICommand SaveAccountCommand { get; }

        public EditAccountWindowViewModel(IAccountsManager accountsManager,ILogger logger, UserAccount editedAccount, IDataValidator dataValidator)
        {
            Account = editedAccount;
            _accountsManager = accountsManager;
            _logger = logger;
            SaveAccountCommand = new RelayCommand((obj) => SaveAccount());
            CloseWindowCommand = new RelayCommand((obj) => CLoseWindow());
            Email = Account.Email;
            Password = Account.Password;
            _dataValidator = dataValidator;
        }

        private readonly IAccountsManager _accountsManager;
        
        private readonly ILogger _logger;

        private readonly IDataValidator _dataValidator;

        public Action Close { get; set; }

        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private UserAccount _account;

        public UserAccount Account
        {
            get => _account;
            set
            {
                _account = value;
                OnPropertyChanged();
            }
        }

        private void SaveAccount()
        {
            if (!_dataValidator.IsEmailValid(Email))
            {
                return;
            }

            _accountsManager.EditAccount(Account,Email,Password);
            
           Close?.Invoke();
        }

        private void CLoseWindow()
        {
            Close?.Invoke();
        }

        public bool CanCloseWindow()
        {
            return true;
        }
    }
}
