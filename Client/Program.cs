using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPMessagingClient
{
    class Client
    {
        private static UdpClient client = new UdpClient();
        private static IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);

        static void Main(string[] args)
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            SendMessage(name, serverEndPoint);

            Task.Run(() =>
            {
                while (true)
                {
                    byte[] data = client.Receive(ref serverEndPoint);
                    string message = Encoding.UTF8.GetString(data);
                    Console.WriteLine(message);
                }
            });

            while (true)
            {
                string message = Console.ReadLine();
                if (message.ToLower() == "/exit")
                {
                    SendMessage(message, serverEndPoint);
                    break;
                }
                SendMessage(message, serverEndPoint);
            }
        }

        static void SendMessage(string message, IPEndPoint endpoint)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, endpoint);
        }
    }
}
