using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.UI.Base;

namespace PixaiBot.Data.Interfaces
{
    internal interface INavigationService
    {
        internal BaseViewModel CurrentView { get; }

        internal void NavigateTo<T>() where T : BaseViewModel;
    }
}
