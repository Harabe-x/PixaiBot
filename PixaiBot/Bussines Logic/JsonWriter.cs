using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace PixaiBot.Bussines_Logic
{
    public class JsonWriter
    {

        public static void WriteJson<T>(T obj, string filePath)
        {
            var serializedJson = JsonSerializer.Serialize(obj);
            File.WriteAllText(filePath,serializedJson);
        }
    }
}
