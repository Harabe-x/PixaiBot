using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management
{
    class ProxyManager : IProxyManager
    {

        private readonly List<string> _proxyList;

        public ProxyManager()
        {
            _proxyList = new List<string>();
        }

        public IEnumerable<string> GetProxyList()
        {
            return _proxyList;
        }

        public string GetRandomProxy()
        {
            if (_proxyList.Count == 0) return "Proxy list is empty";
            return _proxyList[new Random().Next(0, _proxyList.Count)];
        }

        public void ReadProxyFile(string filePath)
        {
            if (!File.Exists(filePath)) return;

            var proxyFile = File.ReadAllLines(filePath);

            const string proxyPattern = @"^(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\s*:\s*(\d{1,5})$";

            foreach (var proxy in proxyFile)
            {
                if(!Regex.IsMatch(proxy,proxyPattern)) continue;

                _proxyList.Add(proxy);
            }

        }
    }
}
