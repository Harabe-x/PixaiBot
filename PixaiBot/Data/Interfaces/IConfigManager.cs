﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Models;

namespace PixaiBot.Data.Interfaces;

public interface IConfigManager
{
    public void SaveConfig(UserConfig config);

    public UserConfig GetConfig();
}