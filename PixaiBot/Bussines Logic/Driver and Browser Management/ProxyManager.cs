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

        private const string ProxyPattern = @"^(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\s*:\s*(\d{1,5})$";

        private readonly Random _random;

        public ProxyManager()
        {
            _proxyList = new List<string>();
            _random = new Random();
        }

        public IEnumerable<string> GetProxyList()
        {
            return _proxyList;
        } 

        public string GetRandomProxy()
        {
            if (_proxyList.Count == 0) return "Proxy list is empty";
            return _proxyList[_random.Next(0, _proxyList.Count)];
        }

        public void ReadProxyFile(string filePath)
        {
            if (!File.Exists(filePath)) return;

            var proxyFile = File.ReadAllLines(filePath);


            foreach (var proxy in proxyFile)
            {
                if (!Regex.IsMatch(proxy, ProxyPattern)) continue;
                if(_proxyList.Contains(proxy)) continue;
                _proxyList.Add(proxy);
            }
        }
    }
}
