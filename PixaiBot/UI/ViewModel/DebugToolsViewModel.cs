using System.Windows.Input;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;
using PixaiBot.UI.View;

namespace PixaiBot.UI.ViewModel;

public class DebugToolsViewModel : BaseViewModel
{
    #region Ctor

    public DebugToolsViewModel(ICreditClaimer creditClaimer, IToastNotificationSender notificationSender,
        IAccountsManager accountManager,
        IConfigManager configManager, IBotStatisticsManager botStatisticsManager, ILogger logger,
        IProxyManager proxyManager, IAccountCreator accountCreator,
        IDialogService dialogService, IDataValidator dataValidator, IAccountLoginChecker accountLoginChecker,
        IRealTimeLogWatcher logWatcher)
    {
        _creditClaimer = creditClaimer;
        _notificationSender = notificationSender;
        _accountManager = accountManager;
        _configManager = configManager;
        _botStatisticsManager = botStatisticsManager;
        _logger = logger;
        _proxyManager = proxyManager;
        _accountCreator = accountCreator;
        _dialogService = dialogService;
        _dataValidator = dataValidator;
        _accountLoginChecker = accountLoginChecker;
        _realTimeLogWatcher = logWatcher;
        OpenDebugLogLivePreviewCommand = new RelayCommand(_ => OpenDebugLogLivePreview());
        RunAccountRegistrationCommand = new RelayCommand(_ => RunAccountRegistration());
        RunAllTasksCommand = new RelayCommand(_ => RunAllTasks());
        RunCreditClaimCommand = new RelayCommand(_ => RunCreditClaim());
        RunAccountLoginCheckCommand = new RelayCommand(_ => RunAccountLoginCheck());
    }

    #endregion

    #region Commands

    public ICommand RunAllTasksCommand { get; }

    public ICommand OpenDebugLogLivePreviewCommand { get; }

    public ICommand RunAccountLoginCheckCommand { get; }

    public ICommand RunAccountRegistrationCommand { get; }

    public ICommand RunCreditClaimCommand { get; }

    #endregion

    #region Methods

    private void OpenDebugLogLivePreview()
    {
        _dialogService.ShowDialog(new LogWindowView(), new LogWindowViewModel(_realTimeLogWatcher), false);
    }

    private void RunAllTasks()
    {
        RunCreditClaim();
        RunAccountRegistration();
        RunAccountLoginCheck();
    }

    private void RunCreditClaim()
    {
        var creditClaimerViewModel = new CreditClaimerViewModel(_creditClaimer, _notificationSender, _accountManager,
            _configManager, _botStatisticsManager, _logger);

        creditClaimerViewModel.ClaimCreditsCommand.Execute(null);
    }

    private void RunAccountLoginCheck()
    {
        var settingsViewModel = new SettingsViewModel(_dialogService, _accountManager, _dataValidator,
            _accountLoginChecker, _logger, _botStatisticsManager, _configManager, _notificationSender);

        settingsViewModel.CheckAllAccountsLoginCommand.Execute(null);
    }

    private void RunAccountRegistration()
    {
        var accountRegistrationViewModel = new AccountCreatorViewModel(_proxyManager, _logger, _accountManager,
            _notificationSender, _accountCreator, _configManager);

        accountRegistrationViewModel.AccountAmount = 1.ToString();

        accountRegistrationViewModel.StartAccountCreationCommand.Execute(null);
    }

    #endregion

    #region Fields

    private readonly ICreditClaimer _creditClaimer;

    private readonly IToastNotificationSender _notificationSender;

    private readonly IAccountsManager _accountManager;

    private readonly IConfigManager _configManager;

    private readonly IBotStatisticsManager _botStatisticsManager;

    private readonly ILogger _logger;

    private readonly IProxyManager _proxyManager;

    private readonly IAccountCreator _accountCreator;

    private readonly IDialogService _dialogService;

    private readonly IDataValidator _dataValidator;

    private readonly IAccountLoginChecker _accountLoginChecker;

    private readonly IRealTimeLogWatcher _realTimeLogWatcher;

    #endregion
}