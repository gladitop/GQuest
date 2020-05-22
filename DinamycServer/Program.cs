using System;
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
            Console.WriteLine("Запуск сервера...");

            var thread1 = new Thread(ClearBadClient);
            thread1.Start();
            server = new TcpListener(IPAddress.Any, Data.Port);
            server.Start();
            var thread = new Thread(ListenClients);
            thread.Start();

            Console.WriteLine("Сервер работает!");

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

            static void ClearBadClient() //Очистка 'Плохих' клиентов
            {
                //TODO: Это надо проверить!
                while (true)
                {
                    Task.Delay(1000).Wait();

                    foreach (var clientInfo in Data.ClientsInfo)
                        try
                        {
                            if (!clientInfo.Socket.Connected)
                            {
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
                            var command = message.Substring(1, message.IndexOf(ch) - 1); //Команда 
                            var arguments = message.Substring(message.IndexOf(ch) + 1).Split(new[] {ch}); //Массив аргументов

                            try
                            {
                                //Обращение к командам

                                #region Как команда от клиенте?

                                Console.WriteLine("\n" + "Команда: " + command + " \n ");
                                Console.WriteLine("Аргументы:\n----------");
                                foreach (var s in arguments) Console.WriteLine(s);
                                Console.WriteLine("----------");

                                #endregion

                                var ComandClass = new Commands();
                                ComandClass.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(ComandClass, new object[] {client, arguments});
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("\nОшибка при поиске команды:\n----------\n" + ex + "\n----------");
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