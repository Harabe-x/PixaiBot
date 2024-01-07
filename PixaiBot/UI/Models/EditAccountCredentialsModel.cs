using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.UI.Models;

internal class EditAccountCredentialsModel
{
    public string? Email { get; set; }

    public string? Password { get; set; }

    public UserAccount Account { get; set; }
}