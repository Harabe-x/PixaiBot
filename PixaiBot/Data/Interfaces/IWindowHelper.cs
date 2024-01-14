using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces;

public interface IWindowHelper
{
    /// <summary>
    /// Closes the window
    /// </summary>
    public Action Close { get; set; }

    /// <summary>
    /// Determines whether the window can be closed.
    /// </summary>
    /// <returns>if window can be closed returns true; otherwise returns false</returns>
    public bool CanCloseWindow();
}