using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Base;

namespace PixaiBot.UI.ViewModel
{
    internal class AccountListViewModel : BaseViewModel
    {

        public AccountListViewModel(IAccountsManager accountsManager)
        {
            _accountsMangaer = accountsManager;
        }

        private readonly IAccountsManager _accountsMangaer;
    }
}
