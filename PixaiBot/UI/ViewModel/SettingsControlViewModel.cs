using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;
using PixaiBot.UI.View;

namespace PixaiBot.UI.ViewModel
{
    internal class SettingsControlViewModel : BaseViewModel
    {
       
        public ICommand ShowAddAccountWindowCommand { get;  }
                    
        
        public SettingsControlViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            ShowAddAccountWindowCommand = new RelayCommand((obj) => ShowAddAccountWindow());



        }

        private readonly IDialogService _dialogService;


        private void ShowAddAccountWindow()
        {
            _dialogService.ShowDialog(new AddAccountWindowView(), true);
        }

    }
}
