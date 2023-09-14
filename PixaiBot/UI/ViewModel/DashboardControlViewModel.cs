using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;

namespace PixaiBot.UI.ViewModel
{
    internal class DashboardControlViewModel : BaseViewModel
    {
        public ICommand testCommand { get;  }

        public DashboardControlViewModel(IAccountLoginChecker accountLoginChecker)
        {
            _accountLoginChecker = accountLoginChecker;
            testCommand = new RelayCommand((obj) => test());
        }

        private readonly IAccountLoginChecker _accountLoginChecker;


        private void test()
        {
            _accountLoginChecker.CheckAccountLogin(null);
        }
        
    }
}
