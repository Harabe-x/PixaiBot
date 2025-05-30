﻿using System;
using System.Windows.Input;
using Notification.Wpf;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;
using PixaiBot.UI.Models;

namespace PixaiBot.UI.ViewModel;

public class AddAccountViewModel : BaseViewModel, IWindowHelper
{
    #region Constructor

    public AddAccountViewModel(IAccountsManager accountsManager, IToastNotificationSender notificationSender,
        IDataValidator dataValidator, ILogger logger, IConfigManager configManager)
    {
        AddAccountCommand = new RelayCommand(_ => AddAccount());
        CloseWindowCommand = new RelayCommand(_ => CloseWindow());
        _addAccountModel = new AddAccountModel();
        _accountsManger = accountsManager;
        _notificationSender = notificationSender;
        _dataValidator = dataValidator;
        _logger = logger;
        _configManager = configManager;
    }

    #endregion

    #region Commands

    public ICommand AddAccountCommand { get; }

    public ICommand CloseWindowCommand { get; }

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
        if (_configManager.GetConfig().ToastNotifications)
            _notificationSender.SendNotification("PixaiBot", "Account added successfully", NotificationType.Success);
        CloseWindow();
        _logger.Log("Account Added ", _logger.ApplicationLogFilePath);
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

    private readonly IToastNotificationSender _notificationSender;

    private readonly IConfigManager _configManager;

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