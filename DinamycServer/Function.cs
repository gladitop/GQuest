using System;
using System.Net.Sockets;
using System.Text;

namespace DinamycServer
{
    public static class Function //Функции
    {
        public static void SendClientMessage(object  client, string message) //Отравить клиенту сообщение
        {
            try
            {
                ((Data.ThreadClient) client).TpClient.Client.Send(Encoding.UTF8.GetBytes($"{message}☼"));
                WriteConsole($"Send: {message}☼");
            }
            catch
            {
                CheckEmptyClients(client);
                WriteConsole($"ERRMESS: {message}", ConsoleColor.Yellow);
            }
        }

        public static void CheckEmptyClients(object client) //Поиск пустых клиентов и их удаление
        {
            if (client != null)
            {
                check(client);
            }
            else
            {
                foreach (var ChClients in Data.Clients) check(ChClients);
                WriteConsole("Clients cleared", ConsoleColor.Yellow);
            }

            void check(object cl) //Метод по проверке определённого клиента
            {
                var info = (Data.ThreadClient)client;

                try
                {
                    info.TpClient.Client.Send(new byte[1]);
                }
                catch
                {
                    Data.Clients.Remove(info);
                    info.TpClient.Close();
                    info.ThrClient.Abort();
                    WriteConsole("Client removed", ConsoleColor.Yellow);
                }
            }
        }

        public static void WriteConsole(string text, ConsoleColor color) //Отправка цветного сообщения в консоль
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            //Data.Logger.WriteLine($"{DateTime.Now}:{text}"); /TODO: доделать
            Console.ResetColor();
        }
        public static void WriteConsole(string text) //Отправка сообщения в консоль
        {
            Console.WriteLine(text);
            //Data.Logger.WriteLine($"{DateTime.Now}:{text}"); /TODO: доделать
        }      
    }
}