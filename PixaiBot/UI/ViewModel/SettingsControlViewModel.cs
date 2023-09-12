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
                    
        
        public SettingsControlViewModel(IDialogService dialogService,IAccountsManager accountsManager,IDataValidator dataValidator)
        {
            _dialogService = dialogService;
            _accountsManager = accountsManager;
            _dataValidator = dataValidator;
            ShowAddAccountWindowCommand = new RelayCommand((obj) => ShowAddAccountWindow());



        }

        private readonly IDialogService _dialogService;

        private readonly IAccountsManager _accountsManager;

        private readonly IDataValidator _dataValidator;


        private void ShowAddAccountWindow()
        {
            _dialogService.ShowDialog(new AddAccountWindowView(_accountsManager,_dataValidator), true);
        }

    }
}
