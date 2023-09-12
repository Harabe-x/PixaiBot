using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic
{
    public class JsonReader
    {
        public IList<UserAccount> ReadAccountFile(string filePath) 
        {
            var jsonString = System.IO.File.ReadAllText(filePath);
            var accounts = JsonSerializer.Deserialize<IList<UserAccount>>(jsonString);
            return accounts;
        }

        public void WriteAccountList(IList<UserAccount> accountsList, string filePath)
        {
            var serialzedText = JsonSerializer.Serialize(accountsList);         
            System.IO.File.WriteAllText(filePath, serialzedText);
        }

    }
}
