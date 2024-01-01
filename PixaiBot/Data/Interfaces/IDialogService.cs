using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PixaiBot.UI.Base;

namespace PixaiBot.Data.Interfaces;

public interface IDialogService
{
    public void ShowDialog<TDialogView, TDialogViewModel>(TDialogView dialogWindowView,
        TDialogViewModel dialogWindowViewModel, bool isModal)
        where TDialogView : Window
        where TDialogViewModel : BaseViewModel;
}