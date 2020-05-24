using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DinamycServer
{
    internal class Program
    {
        public static TcpListener server { get; set; }

        private static void Main(string[] args)
        {
            #region Запуск сервера + консольные команды

            Console.WriteLine("Запуск сервера...");

            //var thread1 = new Thread(ClearBadClient);
            //thread1.Start();
            server = new TcpListener(IPAddress.Any, Data.Port);
            server.Start();
            Data.NetworkStream = new NetworkStream(server.Server, FileAccess.ReadWrite);
            var thread = new Thread(ListenClients);
            thread.Start();

            Function.WriteColorText("Сервер работает!", ConsoleColor.Green);

            var answer = "";
            while (true)
            {
                answer = Console.ReadLine();

                switch (answer)
                {
                    case "stop":
                        Console.WriteLine("Отключение сервера...");
                        server.Stop();
                        break;
                }
            }

            #endregion


            /*static void ClearBadClient() //Очистка 'Плохих' клиентов
             {
                 //TODO: Не работает (23.05.2020)
                 while (true)
                 {
                     Task.Delay(2000).Wait();
                     foreach (var clientInfo in Data.ClientsInfo)
                         try
                         {
                             Console.WriteLine("2");
                             if (!clientInfo.Socket.Connected)
                             {
                                 Console.WriteLine("3");
                                 clientInfo.Socket.Close();
                                 Data.ClientsInfo.Remove(clientInfo);
                                 Console.WriteLine("Найден плохой клиент");
                             }
                         }
                         catch (Exception ex)
                         {
                             clientInfo.Socket.Close();
                             Data.ClientsInfo.Remove(clientInfo);
                             Console.WriteLine($"Ошибка: {ex.Message}");                   
                         }
                 }
             }
             */

            static void ListenClients() //Поиск клиентов(Создание потоков с клиентами)
            {
                while (true)
                {
                    Task.Delay(10).Wait(); //Задержка

                    var client = server.AcceptTcpClient();
                    var thread = new Thread(ClientLog);
                    thread.Start(client);
                }
            }

            static void ClientLog(object obj) //Поток клиента
            {
                var client = (TcpClient) obj;
                var buffer = new byte[1024];
                Console.WriteLine("новое подключение");

                while (true)
                    try
                    {
                        Task.Delay(10).Wait();

                        var i = client.Client.Receive(buffer);
                        var message = Encoding.UTF8.GetString(buffer, 0, i);

                        if (message != "")
                        {
                            var ch = ':'; //Разделяющий символ
                            var ComandClass = new Commands();
                            try
                            {
                                var command = message.Substring(1, message.IndexOf(ch) - 1); //Команда 
                                try
                                {
                                    var arguments =
                                        message.Substring(message.IndexOf(ch) + 1)
                                            .Split(new[] {ch}); //Массив аргументов

                                    #region Вывод в консоль: Команда и аргументы

                                    Console.WriteLine("\n" + "Команда: " + command + " \n ");
                                    Console.WriteLine("Аргументы:\n----------");
                                    foreach (var s in arguments) Console.WriteLine(s);
                                    Console.WriteLine("----------");

                                    #endregion

                                    ComandClass.GetType()
                                        .GetMethod(command, BindingFlags.Instance | BindingFlags.NonPublic)
                                        .Invoke(ComandClass, new object[] {client, arguments});
                                }
                                catch (Exception ex)
                                {
                                    Function.WriteColorText("\n" + "Неверный ввод или ПОПЫТКА ВЗЛОМА",
                                        ConsoleColor.Red);
                                    Console.WriteLine(ex);
                                }
                            }
                            catch
                            {
                                try
                                {
                                    var command = message.Substring(1);
                                    Console.WriteLine("\n" + "Команда: " + command);
                                    ComandClass.GetType()
                                        .GetMethod(command, BindingFlags.Instance | BindingFlags.NonPublic)
                                        .Invoke(ComandClass, new object[] {client});
                                }
                                catch (Exception ex)
                                {
                                    Function.WriteColorText("\n" + "Неверный ввод или ПОПЫТКА ВЗЛОМА",
                                        ConsoleColor.Red);
                                    Console.WriteLine(ex);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
            }
        }
    }
}