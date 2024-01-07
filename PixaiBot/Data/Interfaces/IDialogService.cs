using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PixaiBot.UI.Base;

namespace PixaiBot.Data.Interfaces;

public interface IDialogService
{/// <summary>
///   Binds <paramref name="dialogWindowViewModel"/> with <paramref name="dialogWindowView"/> and shows it;
/// </summary>
/// <typeparam name="TDialogView">Needs to inherit from <see cref="Window"/></typeparam>
/// <typeparam name="TDialogViewModel">Needs to inherit from <see cref="BaseViewModel"/></typeparam>
/// <param name="dialogWindowView">Dialog window view</param>
/// <param name="dialogWindowViewModel">DataContext for the window</param>
/// <param name="isModal">determines whether the dialog window should prevent the use of the application</param>
    public void ShowDialog<TDialogView, TDialogViewModel>(TDialogView dialogWindowView,
        TDialogViewModel dialogWindowViewModel, bool isModal)
        where TDialogView : Window
        where TDialogViewModel : BaseViewModel;
}