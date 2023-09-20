using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;

namespace PixaiBot.UI.Services;

public class NavigationService : BaseViewModel, INavigationService

{
    private readonly Func<Type, BaseViewModel> _viewModelFactory;

    private BaseViewModel _currentView;

    public BaseViewModel CurrentView
    {
        get => _currentView;
        private set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public NavigationService(Func<Type, BaseViewModel> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }


    public void NavigateTo<T>() where T : BaseViewModel
    {
        CurrentView = _viewModelFactory.Invoke(typeof(T));
    }
}