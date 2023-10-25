﻿using System;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Threading;
using PixaiBot.Bussines_Logic;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;
using PixaiBot.UI.Services;
using PixaiBot.UI.View;
using PixaiBot.UI.ViewModel;

namespace PixaiBot;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    private readonly ILogger _logger;

    public App()
    {
        _logger = new Logger();
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<MainWindowView>(provider => new MainWindowView()
            { DataContext = provider.GetService<MainWindowViewModel>() });
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<DashboardControlViewModel>();
        services.AddSingleton<SettingsControlViewModel>();
        services.AddSingleton<AccountListViewModel>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IAccountsManager, AccountsManager>();
        services.AddSingleton<IDataValidator, DataValidator>();
        services.AddSingleton<IConfigManager, ConfigManager>();
        services.AddSingleton<IAccountLoginChecker, AccountLoginChecker>();
        services.AddSingleton<IBotStatisticsManager, BotStatisticsManager>();
        services.AddSingleton<ICreditClaimer, CreditClaimer>();
        services.AddSingleton<ILogger, Logger>();
        services.AddSingleton<IToastNotificationSender, ToastNotificationSender>();
        services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider =>
            viewModelType => (BaseViewModel)serviceProvider.GetRequiredService(viewModelType));
        _serviceProvider = services.BuildServiceProvider();
        Current.DispatcherUnhandledException += HandleUnhandledApplicationException;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        InitialConfiguration.CreateConfigFile();
        InitialConfiguration.CreateStatisticsFile();
        _logger.Log("=====Application Started=====", _logger.ApplicationLogFilePath);
        base.OnStartup(e);
        var mainWindow = _serviceProvider.GetRequiredService<MainWindowView>();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _logger.Log("=====Application Closed=====", _logger.ApplicationLogFilePath);
        base.OnExit(e);
    }

    private void HandleUnhandledApplicationException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        _logger.Log($"{e.Exception} | {e.Exception.Message}", _logger.ApplicationLogFilePath);
        e.Handled = true;
    }
}