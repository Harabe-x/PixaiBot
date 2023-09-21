using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;

namespace PixaiBot.UI.ViewModel;

public class MainWindowViewModel : BaseViewModel
{
    public ICommand NavigateToDashboardCommand { get; }

    public ICommand NavigateToSettingsCommand { get; }

    public ICommand ExitApplicationCommand { get; }

    public ICommand HideApplicationCommand { get; }

    public MainWindowViewModel(INavigationService navService)
    {
        NavigateToDashboardCommand = new RelayCommand((obj) => NavigateToDashboard());
        NavigateToSettingsCommand = new RelayCommand((obj) => NavigateToSettings());
        ExitApplicationCommand = new RelayCommand((obj) => ExitApplication());
        Navigation = navService;
        HideApplicationCommand = new RelayCommand((obj) => HideApplication());
        NavigateToDashboardCommand.Execute(null);

       
    }

    private INavigationService _navigation;

    public INavigationService Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }


    private void NavigateToDashboard()
    {
        Navigation.NavigateTo<DashboardControlViewModel>();
    }

    private void NavigateToSettings()
    {
        Navigation.NavigateTo<SettingsControlViewModel>();
    }

    private static void HideApplication()
    {

    }

    private static void ExitApplication()
    {
        Environment.Exit(0);
    }
    
}