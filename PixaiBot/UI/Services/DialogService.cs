using System.Windows;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;

namespace PixaiBot.UI.Services;

public class DialogService : IDialogService
{
    public void ShowDialog<TDialogView, TDialogViewModel>(TDialogView dialogWindowView,
        TDialogViewModel dialogWindowViewModel, bool isModal)
        where TDialogView : Window
        where TDialogViewModel : BaseViewModel
    {
        dialogWindowView.DataContext = dialogWindowViewModel;

        if (isModal) dialogWindowView.ShowDialog();
        else dialogWindowView.Show();
    }
}