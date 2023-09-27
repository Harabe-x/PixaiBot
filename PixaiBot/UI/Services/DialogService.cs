using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.UI.Services;

public class DialogService : IDialogService
{
    public void ShowDialog<TDialog>(TDialog dialogWindow, bool isModal) where TDialog : Window
    {
        if (isModal)
            dialogWindow.ShowDialog();
        else
            dialogWindow.Show();
    }
}