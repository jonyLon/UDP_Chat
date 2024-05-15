using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPMessagingServer
{
    class Server
    {
        private static int maxParticipants = 5;
        private static List<IPEndPoint> participants = new List<IPEndPoint>();
        private static UdpClient server = new UdpClient(12345);

        static void Main(string[] args)
        {
            Console.WriteLine("Server started");
            Listen();
        }

        static void Listen()
        {
            while (true)
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = server.Receive(ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(data);

                if (message.StartsWith("/exit"))
                {
                    participants.Remove(remoteEndPoint);
                    SendMessage("You left the chat!", remoteEndPoint);
                    continue;
                }

                if (!participants.Contains(remoteEndPoint))
                {
                    if (participants.Count < maxParticipants)
                    {
                        participants.Add(remoteEndPoint);
                        SendMessage("Welcome to the chat!", remoteEndPoint);
                    }
                    else
                    {
                        SendMessage("Chat room is full!", remoteEndPoint);
                        continue;
                    }
                }

                    foreach (var participant in participants)
                    {
                        if (participant != remoteEndPoint)
                        {
                            SendMessage($"{DateTime.Now.ToString("HH:mm:ss")} - {message}", participant);
                        }
                    }
                
            }
        }

        static void SendMessage(string message, IPEndPoint endpoint)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            server.Send(data, data.Length, endpoint);
        }
    }
}
