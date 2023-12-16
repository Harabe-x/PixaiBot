using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;
using PixaiBot.UI.Base;
using PixaiBot.UI.Models;

namespace PixaiBot.UI.ViewModel
{
    internal class EditAccountWindowViewModel : BaseViewModel , IWindowHelper
    {
        #region Commands

        public ICommand CloseWindowCommand { get; }

        public ICommand SaveAccountCommand { get; }

        #endregion
        #region Constructor 
        public EditAccountWindowViewModel(IAccountsManager accountsManager,ILogger logger, UserAccount editedAccount, IDataValidator dataValidator)
        {
            SaveAccountCommand = new RelayCommand((obj) => SaveAccount());
            CloseWindowCommand = new RelayCommand((obj) => CLoseWindow());
            _editAccountModel = new EditAccountModel();
            Account = editedAccount;
            Email = Account.Email;
            Password = Account.Password;
            _dataValidator = dataValidator;
            _accountsManager = accountsManager;
            _logger = logger;
        }
        #endregion
        #region Methods
        private void SaveAccount()
        {
            if (!_dataValidator.IsEmailValid(Email) || !_dataValidator.IsPasswordValid(Password)) return;
            
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
        #endregion
        #region Fields

        public string Email
        {
            get => _editAccountModel.Email;
            set
            {
                _editAccountModel.Email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _editAccountModel.Password;
            set
            {
                _editAccountModel.Password = value;
                OnPropertyChanged();
            }
        }

        public UserAccount Account
        {
            get => _editAccountModel.Account;
            set
            {
                _editAccountModel.Account = value;
                OnPropertyChanged();
            }
        }

        private readonly IAccountsManager _accountsManager;

        private readonly ILogger _logger;

        private readonly IDataValidator _dataValidator;

        private readonly EditAccountModel _editAccountModel;

        public Action Close { get; set; }

        #endregion
    }
}
