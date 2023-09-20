using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Models;

namespace PixaiBot.Data.Interfaces;

internal interface IConfigManager
{
    UserConfig GetConfig();

    void SaveConfig(UserConfig config);
}