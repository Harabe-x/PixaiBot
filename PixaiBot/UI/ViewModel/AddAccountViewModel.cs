using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;
using PixaiBot.UI.Models;

namespace PixaiBot.UI.ViewModel;

public class AddAccountViewModel : BaseViewModel, IWindowHelper
{
    #region Commands

    public ICommand AddAccountCommand { get; }

    public ICommand CloseWindowCommand { get; }

    #endregion

    #region Constructor

    public AddAccountViewModel(IAccountsManager accountsManager, IDataValidator dataValidator, ILogger logger)
    {
        AddAccountCommand = new RelayCommand(_ => AddAccount());
        CloseWindowCommand = new RelayCommand(_ => CloseWindow());
        _addAccountModel = new AddAccountModel();
        _accountsManger = accountsManager;
        _dataValidator = dataValidator;
        _logger = logger;
    }

    #endregion

    #region Methods

    private void CloseWindow()
    {
        _logger.Log("Closing Add Account Window", _logger.ApplicationLogFilePath);

        Close?.Invoke();
    }


    private void AddAccount()
    {
        _logger.Log("Adding new account", _logger.ApplicationLogFilePath);
        if (!_dataValidator.IsEmailValid(Email) || !_dataValidator.IsPasswordValid(Password))
        {
            _logger.Log("Account data validation failed", _logger.ApplicationLogFilePath);
            return;
        }

        var userAccount = new UserAccount
        {
            Email = Email,
            Password = Password
        };
        _accountsManger.AddAccount(userAccount);
        CloseWindow();
        _logger.Log("Account Added", _logger.ApplicationLogFilePath);
    }

    public bool CanCloseWindow()
    {
        return true;
    }

    #endregion

    #region Fields

    public Action Close { get; set; }

    private readonly ILogger _logger;

    private readonly IAccountsManager _accountsManger;

    private readonly AddAccountModel _addAccountModel;

    private readonly IDataValidator _dataValidator;


    public string Email
    {
        get => _addAccountModel.Email;
        set
        {
            _addAccountModel.Email = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _addAccountModel.Password;
        set
        {
            _addAccountModel.Password = value;
            OnPropertyChanged();
        }
    }

    #endregion
}