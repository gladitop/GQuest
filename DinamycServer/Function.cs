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
            foreach (var clientInfo in Data.ClientsInfo)
                if (clientInfo.Socket != null)
                    SendClientMessage(clientInfo.Socket, message);
        }
    }
}