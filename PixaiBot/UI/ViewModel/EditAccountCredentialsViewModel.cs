using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;
using PixaiBot.UI.Models;

namespace PixaiBot.UI.ViewModel;

internal class EditAccountCredentialsViewModel : BaseViewModel, IWindowHelper
{
    #region Commands

    public ICommand CloseWindowCommand { get; }

    public ICommand SaveAccountCommand { get; }

    #endregion

    #region Constructor

    public EditAccountCredentialsViewModel(IAccountsManager accountsManager, ILogger logger,
        UserAccount editedAccount, IDataValidator dataValidator)
    {
        SaveAccountCommand = new RelayCommand(_ => SaveAccount());
        CloseWindowCommand = new RelayCommand(_ => CLoseWindow());

        _editAccountCredentialsModel = new EditAccountCredentialsModel();
        _dataValidator = dataValidator;
        _accountsManager = accountsManager;
        _logger = logger;

        Account = editedAccount;
        Email = Account.Email;
        Password = Account.Password;
    }

    #endregion

    #region Methods

    private void SaveAccount()
    {
        _logger.Log("Editing account", _logger.CreditClaimerLogFilePath);

        if (!_dataValidator.IsEmailValid(Email) || !_dataValidator.IsPasswordValid(Password)) return;

        _accountsManager.EditAccount(Account, Email, Password);
        _logger.Log("Account edited", _logger.CreditClaimerLogFilePath);
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
        get => _editAccountCredentialsModel.Email;
        set
        {
            _editAccountCredentialsModel.Email = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _editAccountCredentialsModel.Password;
        set
        {
            _editAccountCredentialsModel.Password = value;
            OnPropertyChanged();
        }
    }

    public UserAccount Account
    {
        get => _editAccountCredentialsModel.Account;
        set
        {
            _editAccountCredentialsModel.Account = value;
            OnPropertyChanged();
        }
    }

    private readonly IAccountsManager _accountsManager;

    private readonly ILogger _logger;

    private readonly IDataValidator _dataValidator;

    private readonly EditAccountCredentialsModel _editAccountCredentialsModel;

    public Action Close { get; set; }

    #endregion
}