using System;
using System.Windows.Input;
using Notification.Wpf;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;
using PixaiBot.UI.Models;

namespace PixaiBot.UI.ViewModel;

internal class EditAccountCredentialsViewModel : BaseViewModel, IWindowHelper
{
    #region Constructor

    public EditAccountCredentialsViewModel(IAccountsManager accountsManager, ILogger logger,
        UserAccount editedAccount, IDataValidator dataValidator, IToastNotificationSender notificationSender,
        IConfigManager configManager)
    {
        SaveAccountCommand = new RelayCommand(_ => SaveAccount());
        CloseWindowCommand = new RelayCommand(_ => CLoseWindow());

        _editAccountCredentialsModel = new EditAccountCredentialsModel();
        _dataValidator = dataValidator;
        _accountsManager = accountsManager;
        _logger = logger;
        _notificationSender = notificationSender;
        _configManager = configManager;
        Account = editedAccount;
        Email = Account.Email;
        Password = Account.Password;
    }

    #endregion

    #region Commands

    public ICommand CloseWindowCommand { get; }

    public ICommand SaveAccountCommand { get; }

    #endregion

    #region Methods

    private void SaveAccount()
    {
        _logger.Log("Editing account ", _logger.CreditClaimerLogFilePath);

        if (!_dataValidator.IsEmailValid(Email) || !_dataValidator.IsPasswordValid(Password)) return;

        _accountsManager.EditAccount(Account, Email, Password);
        _logger.Log("Account edited successfully", _logger.CreditClaimerLogFilePath);
        if (_configManager.GetConfig().ToastNotifications)
            _notificationSender.SendNotification("PixaiBot", "Account edited successfully", NotificationType.Success);
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

    private readonly IToastNotificationSender _notificationSender;

    private readonly IConfigManager _configManager;

    private readonly IDataValidator _dataValidator;

    private readonly EditAccountCredentialsModel _editAccountCredentialsModel;

    public Action Close { get; set; }

    #endregion
}