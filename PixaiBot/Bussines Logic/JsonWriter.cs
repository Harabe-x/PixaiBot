using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace PixaiBot.Bussines_Logic
{
    internal class JsonWriter
    {

        public void WriteJson<T>(T obj, string filePath)
        {
            var serializedJson = JsonSerializer.Serialize(obj);
            File.WriteAllText(filePath,serializedJson);
        }
    }
}
