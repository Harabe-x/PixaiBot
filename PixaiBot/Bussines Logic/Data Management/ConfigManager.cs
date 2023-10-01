using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic;

public class ConfigManager : IConfigManager
{

    private readonly JsonReader _jsonReader;

    private readonly ILogger _logger;

    private UserConfig _userConfig;
    
    public string ConfigFilePath { get; }

    public event EventHandler? ConfigChanged;


    private bool _shouldStartWithSystem;

    public bool ShouldStartWithSystem
    {
        get => _shouldStartWithSystem;
        private set
        {
            if (_shouldStartWithSystem == value) return;
            _shouldStartWithSystem = value;
            _userConfig.StartWithSystem = value;
              SaveConfig(_userConfig);
        }
    }

    private bool _shouldSendToastNotifications;

    public bool ShouldSendToastNotifications
    {
        get => _shouldSendToastNotifications;
        private set
        {
            if (_shouldSendToastNotifications == value) return;
            _shouldSendToastNotifications = value;
            _userConfig.ToastNotifications = value;
             SaveConfig(_userConfig);
        }

    }

    private bool _shouldAutoClaimCredits;

    public bool ShouldAutoClaimCredits
    {
        get => _shouldAutoClaimCredits;
        private set
        {
            if (_shouldAutoClaimCredits == value) return;
            _shouldAutoClaimCredits = value;
            _userConfig.CreditsAutoClaim = value;
          SaveConfig(_userConfig);
        }

    }

    private void InitializeData()
    {
        _userConfig = GetConfig();
        ShouldAutoClaimCredits = _userConfig.StartWithSystem;
        ShouldSendToastNotifications = _userConfig.ToastNotifications;
        ShouldAutoClaimCredits = _userConfig.CreditsAutoClaim;
    }
    

    public ConfigManager(ILogger logger)
    {
        _logger = logger;
        ConfigFilePath = InitialConfiguration.UserConfigPath;
        _jsonReader = new JsonReader();
        InitializeData();
    }

    private UserConfig GetConfig()
    {
        _logger.Log("Readed Config File", _logger.ApplicationLogFilePath);
        return _jsonReader.ReadConfigFile(ConfigFilePath);
    }


    public void SaveConfig(UserConfig config)
    {
        _logger.Log("Writed Config File", _logger.ApplicationLogFilePath);
        JsonWriter.WriteJson(config, ConfigFilePath);
    }

    public void SetStartWithSystemFlag(bool flag)
    {
        ShouldStartWithSystem = flag;
        ConfigChanged?.Invoke(this,EventArgs.Empty);
    }

    public void SetToastNotificationsFlag(bool flag)
    {
        ShouldSendToastNotifications = flag;
        ConfigChanged?.Invoke(this,EventArgs.Empty);
    }

    public void SetCreditsAutoClaimFlag(bool flag)
    {
        ShouldAutoClaimCredits = flag;
        ConfigChanged?.Invoke(this,EventArgs.Empty);
    }
}