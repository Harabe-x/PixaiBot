﻿using System;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;
using PixaiBot.UI.Services;
using PixaiBot.UI.View;
using PixaiBot.UI.ViewModel;

namespace PixaiBot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        public App()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<MainWindowView>(provider => new MainWindowView()
                { DataContext = provider.GetService<MainWindowViewModel>() }); 
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<DashboardControlViewModel>();
            services.AddSingleton<SettingsControlViewModel>();
            services.AddSingleton<INavigationService,NavigationService>();
            services.AddSingleton<IDialogService,DialogService>();
            services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider =>
                viewModelType => (BaseViewModel)serviceProvider.GetRequiredService(viewModelType));

            _serviceProvider = services.BuildServiceProvider();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
 
            var mainWindow = _serviceProvider.GetRequiredService<MainWindowView>();
            mainWindow.Show();

        } 
    }
}