using System;
using System.Net.Sockets;
using System.Text;

namespace DinamycServer
{
    public static class Function //Функции
    {
        public static void SendClientMessage(TcpClient client, string message) //Отравить клиенту сообщение
        {
            if (client != null)
                client.Client.Send(Encoding.UTF8.GetBytes(message));
        }

        public static void WriteColorText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void SendMessage(string message) //Отправить всем сообщение
        {
            Console.WriteLine("2");
            Console.WriteLine(Data.ClientsInfo.Count);
            foreach (var clientInfo in Data.ClientsInfo)
            {
                Console.WriteLine("3");
                if (clientInfo.Socket != null)
                {
                    SendClientMessage(clientInfo.Socket, $"%MES:{message}");
                    Console.WriteLine("1");
                    Console.WriteLine(clientInfo.Socket);
                }
            }
        }
    }
}