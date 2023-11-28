using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management.WebNavigationCore.WebNavigationCoreException
{
    internal class ChromeDriverException : Exception
    {
        
        public ChromeDriverException() { }
        
        public ChromeDriverException(string message) : base(message) { }
     
        public ChromeDriverException(string message, Exception innerException) : base(message, innerException) { }

    }
}
