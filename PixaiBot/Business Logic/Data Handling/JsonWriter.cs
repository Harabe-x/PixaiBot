using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PixaiBot.Business_Logic.Data_Handling;

public static class JsonWriter
{
    static JsonWriter()
    {
        Options = new JsonSerializerOptions
        {   
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };
    }
    
    
    /// <summary>
    ///     Writes a json file with the given object.
    /// </summary>
    /// <typeparam name="T">Type of object to write.</typeparam>
    /// <param name="obj">Object to write.</param>
    /// <param name="filePath">Destination file path.</param>
    public static void WriteJson<T>(T obj, string filePath)
    {
        var serializedJson = JsonSerializer.Serialize(obj, Options);
        File.WriteAllText(filePath, serializedJson,Encoding.UTF8);
    }   
    
    private static readonly  JsonSerializerOptions Options;
}