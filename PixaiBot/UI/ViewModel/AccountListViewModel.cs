using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Notification.Wpf;
using PixaiBot.Data.Interfaces;
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
        IDataValidator DataValidator,IToastNotificationSender notificationSender,IConfigManager configManager)
    {
        RemoveAccountCommand = new RelayCommand(_ => RemoveAccount());
        EditAccountCommand = new RelayCommand(_ => EditAccount());
        _dataValidator = DataValidator;
        _accountsManager = accountsManager;
        _dialogService = dialogService;
        _notificationSender = notificationSender;
        _configManager = configManager;
        _logger = logger;
        _accountListModel = new AccountListModel();
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
        if (_configManager.GetConfig().ToastNotifications)
            _notificationSender.SendNotification("PixaiBot", "Account deleted successfully", NotificationType.Success);
    }

    private void EditAccount()
    {
        _logger.Log("Edit account command called", _logger.ApplicationLogFilePath);

        if (SelectedAccount == null) return;

        _dialogService.ShowDialog(new EditAccountCredentialsView(),
            new EditAccountCredentialsViewModel(_accountsManager, _logger, SelectedAccount, _dataValidator,_notificationSender,_configManager), true);
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

    private readonly IConfigManager _configManager;

    private readonly IToastNotificationSender _notificationSender;

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