using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.UI.Base;

namespace PixaiBot.Data.Interfaces;

public interface INavigationService
{
    
    /// <summary>
    ///  Holds the current view.
    /// </summary>
    public BaseViewModel CurrentView { get; }

    /// <summary>
    /// Navigates to the specified view.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void NavigateTo<T>() where T : BaseViewModel;
}