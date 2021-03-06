using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WorkerServer
{
    public static class Function
    {
        public static void SendClientMessage(object client, string message) //Отравить клиенту сообщение
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

        public static void SendClientMessage(object client, List<string> message) //Отравить клиенту большое сообщение
        {
            try
            {
                var cl = ((Data.ThreadClient) client).TpClient;
                cl.Client.Send(Encoding.UTF8.GetBytes("%CMSG"));
                Task.Delay(10).Wait();

                foreach (var msg in message)
                {
                    cl.Client.Send(Encoding.UTF8.GetBytes($"{msg}☼"));
                    WriteConsole(msg);
                    Task.Delay(10).Wait();
                }

                cl.Client.Send(Encoding.UTF8.GetBytes("%EMSG"));
                Task.Delay(10).Wait();
            }
            catch (Exception ex)
            {
                CheckEmptyClients(client);
                WriteConsole($"ERROR_SCM: {ex}", ConsoleColor.Red);
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
                var info = (Data.ThreadClient) client;

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
            if (color == ConsoleColor.Red)
                Data._logger.LogError(text, DateTimeOffset.Now);
            else if (color == ConsoleColor.Yellow)
                Data._logger.LogWarning(text, DateTimeOffset.Now);
            else if (color == ConsoleColor.Green)
                Data._logger.LogInformation(text, DateTimeOffset.Now);
            else
                Data._logger.LogDebug(text, DateTimeOffset.Now);
            Console.ResetColor();
        }

        public static void WriteConsole(string text) //Отправка сообщения в консоль
        {
            Data._logger.LogInformation(text, DateTimeOffset.Now);
        }
    }
}