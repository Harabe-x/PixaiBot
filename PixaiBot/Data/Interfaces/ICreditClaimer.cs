using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Bussines_Logic;
using PixaiBot.Data.Models;

namespace PixaiBot.Data.Interfaces;

public interface ICreditClaimer
{
    public void ClaimCredits(UserAccount account, IToastNotificationSender toastNotificationSender = null);
}