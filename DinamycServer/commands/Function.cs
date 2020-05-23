using System.Net.Sockets;
using System.Text;
using System;

namespace DinamycServer
{
    public static class Function //Функции
    {
        public static void SendClientMessage(TcpClient client, string message) //Отравить клиенту сообщение
        {
            client.Client.Send(Encoding.UTF8.GetBytes(message));
        }

        public static void WriteColorText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}