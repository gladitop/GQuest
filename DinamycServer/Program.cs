using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;

namespace DinamycServer
{
    internal class Program
    {
        public static TcpListener server { get; set; }

        private static void Main(string[] args)
        {
            #region Запуск сервера + консольные команды

            Console.WriteLine("Start server...");
            Data.Logger = new StreamWriter("LOG.txt", true);
            Data.Logger.AutoFlush = true;
            server = new TcpListener(IPAddress.Any, Data.Port);
            server.Start();
            var thread = new Thread(ListenClients);
            thread.Start();

            Function.WriteConsole("Done server!", ConsoleColor.Green);

            var answer = "";
            while (true)
            {
                answer = Console.ReadLine();

                switch (answer.ToLower())
                {
                    case "stop": //Остановка сервера (После этого ctr + c)
                        Function.WriteConsole("off server...");

                    start:
                        if (server.Pending())
                        {
                            Function.WriteConsole("Waiting...", ConsoleColor.Yellow);
                            Task.Delay(100).Wait();
                            goto start;
                        }

                        server.Stop();
                        Function.WriteConsole("Done off server!", ConsoleColor.Green);
                        Data.Logger.Close();
                        Environment.Exit(0);
                        break;
                    default:
                        Function.WriteConsole("No command!", ConsoleColor.Red);
                        break;
                }
            }

            #endregion

            #region Клиент

            static void ListenClients() //Поиск клиентов(Создание потоков с клиентами)
            {
                while (true)
                {
                start:
                    Task.Delay(10).Wait();

                    if (!server.Pending())
                    {
                        Task.Delay(500).Wait();
                        goto start;
                    }

                    var client = server.AcceptTcpClient(); //клиент
                    var thread = new Thread(ClientLog); //поток

                    Data.Clients.Add(new Data.ThreadClient(client, thread)); //заносим в массив
                    thread.Start(new Data.ThreadClient(client, thread));
                }
            }

            static void ClientLog(object obj) //Поток клиента
            {
                var info = (Data.ThreadClient)obj;
                var TpClient = info.TpClient;
                var TrClient = info.ThrClient;
                List<byte> buffer = new List<byte>();

                Function.WriteConsole("new connect!", ConsoleColor.Cyan);

                while (true)
                {
                end:
                    Task.Delay(10).Wait();

                    var i = TpClient.Client.Receive(buffer.ToArray());
                    var message = Encoding.UTF8.GetString(buffer.ToArray(), 0, i);

                    if (!message.Contains("%")) goto end; //проверка на пустое сообщение
                    message.Substring(message.IndexOf('%'));
                    Function.WriteConsole(message);

                    var ch = ':'; //Разделяющий символ
                    var command = message.Substring(1, message.IndexOf(ch) - 1); //Команда 
                    var arguments = message.Substring(message.IndexOf(ch) + 1).Split(new[] { ch }); //Массив аргументов

                    var ComandClass = new Commands();

                    foreach (var method in ComandClass.GetType().GetTypeInfo().GetMethods())
                    {
                        if (method.Name == command)
                        {
                            ComandClass.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.Public).Invoke(ComandClass, new object[] { obj, arguments });
                            Function.WriteConsole(message, ConsoleColor.Green);
                            goto end;
                        }
                    }
                    Function.WriteConsole($"Error_1: команда:{command}", ConsoleColor.Red);
                }
            }

            #endregion
        }
    }
}