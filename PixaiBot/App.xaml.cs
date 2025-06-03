using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using PixaiBot.Business_Logic.Data_Handling;
using PixaiBot.Business_Logic.Data_Management;
using PixaiBot.Business_Logic.Driver_and_Browser_Management;
using PixaiBot.Business_Logic.Driver_and_Browser_Management.WebNavigationCore;
using PixaiBot.Business_Logic.Logging;
using PixaiBot.Business_Logic.Notifications;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;
using PixaiBot.UI.Services;
using PixaiBot.UI.View;
using PixaiBot.UI.ViewModel;

namespace PixaiBot;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ILogger _logger;
    private readonly ServiceProvider _serviceProvider;

    public App()
    {

        _logger = new Logger();
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton(provider => new NavigationPanelView
            { DataContext = provider.GetService<NavigationPanelViewModel>() });
        services.AddSingleton<NavigationPanelViewModel>();
        services.AddSingleton<CreditClaimerViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<AccountListViewModel>();
        services.AddSingleton<AccountInfoLoggerViewModel>();
        services.AddSingleton<AccountCreatorViewModel>();
        services.AddSingleton<DebugToolsViewModel>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IAccountsManager, AccountsManager>();
        services.AddSingleton<IDataValidator, DataValidator>();
        services.AddSingleton<IConfigManager, ConfigManager>();
        services.AddSingleton<IAccountLoginChecker, AccountLoginChecker>();
        services.AddSingleton<IBotStatisticsManager, BotStatisticsManager>();
        services.AddSingleton<ICreditClaimer, CreditClaimerV2>();
        services.AddSingleton<ILogger, Logger>();
        services.AddSingleton<ILoginCredentialsMaker, LoginCredentialsMaker>();
        services.AddSingleton<IToastNotificationSender, ToastNotificationSender>();
        services.AddSingleton<IProxyManager, ProxyManager>();
        services.AddSingleton<ITempMailApiManager, TempMailApiManager>();
        services.AddSingleton<IRealTimeLogWatcher, RealTimeLogWatcher>();
        services.AddSingleton<IAccountCreator, AccountCreatorV2>();
        services.AddSingleton<IPixaiDataReader, PixaiDataReader>();
        services.AddSingleton<IPixaiNavigation, PixaiNavigation>();
        services.AddSingleton<IAccountInfoLogger, AccountInfoLogger>();
        services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider =>
            viewModelType => (BaseViewModel)serviceProvider.GetRequiredService(viewModelType));
        _serviceProvider = services.BuildServiceProvider();
        Current.DispatcherUnhandledException += HandleUnhandledApplicationException;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        Configuration.CreateConfigFile();
        Configuration.CreateStatisticsFile();
        Configuration.CreateApiKeysFile();
        _logger.Log("=====Application Started=====", _logger.ApplicationLogFilePath);
        base.OnStartup(e);
        var mainWindow = _serviceProvider.GetRequiredService<NavigationPanelView>();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        _logger.Log("=====Application Closed=====\n", _logger.ApplicationLogFilePath);
    }
    
    private void HandleUnhandledApplicationException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        _logger.Log($"{e.Exception} | {e.Exception.Message} | {e.Exception.InnerException}",
            _logger.ApplicationLogFilePath);
        e.Handled = true;
    }
}   