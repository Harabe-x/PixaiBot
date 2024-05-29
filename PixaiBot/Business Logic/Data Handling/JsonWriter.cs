using System.IO;
using System.Text.Json;

namespace PixaiBot.Business_Logic.Data_Handling;

public class JsonWriter
{
    /// <summary>
    ///     Writes a json file with the given object.
    /// </summary>
    /// <typeparam name="T">Type of object to write.</typeparam>
    /// <param name="obj">Object to write.</param>
    /// <param name="filePath">Destination file path.</param>
    public static void WriteJson<T>(T obj, string filePath)
    {
        var serializedJson = JsonSerializer.Serialize(obj);
        File.WriteAllText(filePath, serializedJson);
    }
}