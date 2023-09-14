 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces
{
    public interface IDataValidator 
    {
        public bool ValidateEmail(string email);

        public bool ValidatePassword(string password);
    }
}
