using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Models;

namespace PixaiBot.UI.Models
{
    class AccountListModel
    {
        public ObservableCollection<UserAccount> UserAccounts { get; set; }

        public UserAccount SelectedAccount { get; set; }
    }
}
