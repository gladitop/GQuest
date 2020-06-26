using System;
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
                        server.Stop();
                        Function.WriteConsole("Done off server!", ConsoleColor.Green);
                        //Data.Logger.Close(); TODO: доделать
                        Environment.Exit(0);
                    break;
                }
            }

            #endregion

            #region Клиент

            static void ListenClients() //Поиск клиентов(Создание потоков с клиентами)
            {
                while (true)
                {
                    Task.Delay(10).Wait();

                    var client = server.AcceptTcpClient(); //клиент
                    var thread = new Thread(ClientLog); //поток

                    Data.Clients.Add(new Data.ThreadClient(client, thread)); //заносим в массив
                    thread.Start(new Data.ThreadClient(client, thread));
                }
            }

            static void ClientLog(object obj) //Поток клиента
            {
                var info = (Data.ThreadClient) obj;
                var TpClient = info.TpClient;
                var TrClient = info.ThrClient;
                var buffer = new byte[1024];

                Function.WriteConsole("new connect!", ConsoleColor.Cyan);

                while (true)
                {
                    end:
                    Task.Delay(10).Wait();

                    var i = TpClient.Client.Receive(buffer);
                    var message = Encoding.UTF8.GetString(buffer, 0, i);

                    if(!message.Contains("%")) goto end; //проверка на пустое сообщение
                    message.Substring(message.IndexOf('%'));
                    Function.WriteConsole(message);

                    var ch = ':'; //Разделяющий символ
                    var command = message.Substring(1, message.IndexOf(ch) - 1); //Команда 
                    var arguments = message.Substring(message.IndexOf(ch) + 1).Split(new[] {ch}); //Массив аргументов

                    var ComandClass = new Commands();

                    foreach (var method in ComandClass.GetType().GetTypeInfo().GetMethods())
                    {
                        if(method.Name == command)
                        {
                            ComandClass.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.Public).Invoke(ComandClass, new object[] {obj, arguments});
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