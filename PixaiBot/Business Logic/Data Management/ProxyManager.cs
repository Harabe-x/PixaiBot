﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Business_Logic.Data_Management;

internal class ProxyManager : IProxyManager
{
    #region Constructor

    public ProxyManager(ILogger logger)
    {
        _logger = logger;

        _proxyList = new List<string>();
        _random = new Random();
    }

    #endregion

    #region Methods

    public IEnumerable<string> GetProxyList()
    {
        return _proxyList;
    }

    /// <summary>
    ///     Returns random proxy from proxy list.
    /// </summary>
    /// <returns></returns>
    public string GetRandomProxy()
    {
        _logger.Log("Returning one random proxy", _logger.ApplicationLogFilePath);
        return _proxyList.Count == 0 ? "Proxy list is empty" : _proxyList[_random.Next(0, _proxyList.Count)];
    }


    /// <summary>
    ///     Reads proxy file and adds proxies to proxy list.
    /// </summary>
    /// <param name="filePath"></param>
    public void ReadProxyFile(string filePath)
    {
        _logger.Log("Reading proxy list", _logger.ApplicationLogFilePath);

        if (!File.Exists(filePath)) return;

        var proxyFile = File.ReadAllLines(filePath);


        foreach (var proxy in proxyFile)
        {
            if (!Regex.IsMatch(proxy, ProxyPattern)) continue;
            if (_proxyList.Contains(proxy)) continue;
            _proxyList.Add(proxy);
        }
    }

    #endregion

    #region Fields

    private readonly List<string> _proxyList;

    private const string ProxyPattern = @"^(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\s*:\s*(\d{1,5})$";

    private readonly Random _random;

    private readonly ILogger _logger;

    #endregion
}