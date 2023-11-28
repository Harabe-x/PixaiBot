using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management.WebNavigationCore.WebNavigationCoreException
{
    internal class InvalidPageContentException : Exception
    {
       
        public InvalidPageContentException() { }

        public InvalidPageContentException(string message) : base(message) { }

        public InvalidPageContentException(string message, Exception innerException) : base(message, innerException) { }

    }
}
