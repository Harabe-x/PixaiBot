using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces;

public interface IDataValidator
{
    /// <summary>
    /// checks whether the <paramref name="email"/> is valid.
    /// </summary>
    /// <param name="email">Email to validate</param>
    /// <returns>If <paramref name="email"/> is correct returns true; otherwise returns false</returns>
    public bool IsEmailValid(string email);

    /// <summary>
    /// checks whether the <paramref name="password"/> is valid
    /// </summary>
    /// <param name="password">Password to validate</param>
    /// <returns>If <paramref name="password"/> is correct returns true; otherwise returns false</returns>
    public bool IsPasswordValid(string password);
}