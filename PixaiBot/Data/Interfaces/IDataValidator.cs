using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces;

public interface IDataValidator
{
    public bool IsEmailValid(string email);

    public bool IsPasswordValid(string password);
}