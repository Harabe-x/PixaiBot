using System.Collections.ObjectModel;

namespace PixaiBot.UI.Models;

internal class AccountListModel
{
    public ObservableCollection<UserAccount> UserAccounts { get; set; }

    public UserAccount SelectedAccount { get; set; }
}