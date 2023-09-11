using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PixaiBot.Data.Interfaces
{
    interface IDialogService
    {
        public void ShowDialog<TDialog>(TDialog dialogWindow,bool isModal) where TDialog : Window,new();
    }
}
