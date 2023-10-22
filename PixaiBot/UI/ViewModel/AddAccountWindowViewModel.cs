using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;
using PixaiBot.UI.Base;

namespace PixaiBot.UI.ViewModel;

public class AddAccountWindowViewModel : BaseViewModel, IWindowHelper
{
    public ICommand AddAccountCommand { get; }

    public ICommand CloseWindowCommand { get; }


    public AddAccountWindowViewModel(IAccountsManager accountsManager, IDataValidator dataValidator, ILogger logger)
    {
        _logger = logger;
        _accountsManger = accountsManager;
        _dataValidator = dataValidator;
        AddAccountCommand = new RelayCommand((obj) => AddAccount());
        CloseWindowCommand = new RelayCommand((obj) => CloseWindow());
    }

    public Action Close { get; set; }

    public bool CanCloseWindow()
    {
        return true;
    }

    private readonly ILogger _logger;

    private readonly IAccountsManager _accountsManger;

    private readonly IDataValidator _dataValidator;

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
        _logger.Log("Added Account", _logger.ApplicationLogFilePath);
    }
}