using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.UI.Base;

namespace PixaiBot.Data.Interfaces;

public interface INavigationService
{
    public BaseViewModel CurrentView { get; }

    public void NavigateTo<T>() where T : BaseViewModel;
}