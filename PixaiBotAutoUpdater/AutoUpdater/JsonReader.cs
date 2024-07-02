using System.IO;
using System.Text.Json;

namespace PixaiBotAutoUpdater.AutoUpdater;

internal static class JsonReader
{
    public static ApplicationVersion ReadApplicationVersion(string filePath)
    {
        var jsonContent = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<ApplicationVersion>(jsonContent);
    }
}