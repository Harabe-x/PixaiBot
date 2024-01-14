using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces;

public interface IProxyManager
{
    /// <summary>
    /// Gets the proxy list.
    /// </summary>
    /// <returns>Proxy list</returns>
    IEnumerable<string> GetProxyList();

    /// <summary>
    /// Gets a random proxy from the proxy list.
    /// </summary>
    /// <returns>Random proxy from a list</returns>
    string GetRandomProxy();

    /// <summary>
    /// Reads a proxy file and adds the proxies to the proxy list.
    /// </summary>
    /// <param name="filePath">Proxy file path</param>
    void ReadProxyFile(string filePath);
}