using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PixaiBotAutoUpdater.AutoUpdater
{
    internal static class JsonWriter
    {
        /// <summary>
        /// Writes a json file with the given object.
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
}
