using PixaiBot.UI.Models;

namespace PixaiBot.Data.Interfaces;

public interface IConfigManager
{
    /// <summary>
    ///     Saves the config to the file.
    /// </summary>
    /// <param name="config">Config to write</param>
    public void SaveConfig(UserConfig config);

    /// <summary>
    ///     Gets the config from the file.
    /// </summary>
    /// <returns> <see cref="UserConfig" /> which represents the application configuration></returns>
    public UserConfig GetConfig();
}