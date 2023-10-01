using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Models;

namespace PixaiBot.Data.Interfaces;

public interface IConfigManager
{
    event EventHandler ConfigChanged;

    bool ShouldStartWithSystem { get; }

    bool ShouldSendToastNotifications { get; }

    bool ShouldAutoClaimCredits { get; }

    void SaveConfig(UserConfig config);

    void SetStartWithSystemFlag(bool flag);

    void SetToastNotificationsFlag(bool flag);

    void SetCreditsAutoClaimFlag(bool flag);
}