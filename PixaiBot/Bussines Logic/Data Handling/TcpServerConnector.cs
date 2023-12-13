using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Bussines_Logic.Data_Handling
{
    //TODO: Safely remove this class    
    internal class TcpServerConnector : ITcpServerConnector
    {
        public void SendMessage(string message)
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 4545);

                byte[] data = Encoding.ASCII.GetBytes(message);

                NetworkStream stream = client.GetStream();

                stream.Write(data, 0, data.Length);

                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd: " + ex.Message);
            }
        }
    }
}
