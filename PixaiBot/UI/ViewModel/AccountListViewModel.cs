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
using PixaiBot.UI.Models;
using PixaiBot.UI.View;

namespace PixaiBot.UI.ViewModel;

internal class AccountListViewModel : BaseViewModel
{
    #region Commands

    public ICommand EditAccountCommand { get; }

    public ICommand RemoveAccountCommand { get; }

    #endregion

    #region Constructor

    public AccountListViewModel(IAccountsManager accountsManager, ILogger logger, IDialogService dialogService,
        IDataValidator DataValidator)
    {
        _dataValidator = DataValidator;
        _accountsManager = accountsManager;
        _dialogService = dialogService;
        _logger = logger;
        _accountListModel = new AccountListModel();
        RemoveAccountCommand = new RelayCommand((obj) => RemoveAccount());
        EditAccountCommand = new RelayCommand((obj) => EditAccount());
        UserAccounts = new ObservableCollection<UserAccount>(_accountsManager.GetAllAccounts());
        _accountsManager.AccountsListChanged += AccountsManagerOnAccountsListChanged;
    }

    #endregion

    #region Methods

    private void RemoveAccount()
    {
        if (SelectedAccount == null) return;


        _logger.Log("Remove account command called", _logger.ApplicationLogFilePath);
        _accountsManager.RemoveAccount(SelectedAccount);
    }

    private void EditAccount()
    {
        _logger.Log("Edit account command called", _logger.ApplicationLogFilePath);

        if (SelectedAccount == null) return;

        if (string.IsNullOrEmpty(SelectedAccount.Email) || string.IsNullOrEmpty(SelectedAccount.Password)) return;

        _dialogService.ShowDialog(new EditAccountCredentialsView(),
            new EditAccountCredentialsViewModel(_accountsManager, _logger, SelectedAccount, _dataValidator), true);
    }

    private void AccountsManagerOnAccountsListChanged(object? sender, EventArgs e)
    {
        UserAccounts = new ObservableCollection<UserAccount>(_accountsManager.GetAllAccounts());
        _logger.Log("Accounts list refreshed", _logger.ApplicationLogFilePath);
    }

    #endregion

    #region Fields

    private readonly ILogger _logger;

    private readonly IDialogService _dialogService;

    private readonly IDataValidator _dataValidator;

    private readonly IAccountsManager _accountsManager;

    private readonly AccountListModel _accountListModel;

    public UserAccount SelectedAccount
    {
        get => _accountListModel.SelectedAccount;
        set
        {
            _accountListModel.SelectedAccount = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<UserAccount> UserAccounts
    {
        get => _accountListModel.UserAccounts;
        set
        {
            _accountListModel.UserAccounts = value;
            OnPropertyChanged();
        }
    }

    #endregion
}