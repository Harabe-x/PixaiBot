using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces
{
    internal interface ILogger
    {
        
        string LogFilePath { get; }

        void Log(string message);

    }
}
