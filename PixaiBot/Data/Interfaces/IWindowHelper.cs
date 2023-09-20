using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces;

internal interface IWindowHelper
{
    Action Close { get; set; }

    bool CanCloseWindow { get; set; }
}