using System;
using System.Reflection;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DinamycServer
{
    class Program
    {
        public static TcpListener server { get; set; }

        static void Main(string[] args)
        {            
            //Database = new Database();
            //var thread1 = new Thread(ClearBadClient);
            //thread1.Start();
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

            /*static void ClearBadClient() //Очистка 'Плохих' клиентов //TODO(не работает)
            {
                while (true)
                {
                    Task.Delay(1000).Wait();

                    foreach (var clientInfo in Data.ClientsInfo)
                    {
                        try
                        {
                            Console.WriteLine("1");
                            var ping = new Ping();
                            var status = ping.Send(clientInfo.IP.Address);
                            Console.WriteLine("2");
                            if (status.Status != IPStatus.Success)
                            {
                                Console.WriteLine("3");
                                clientInfo.Socket.Close();
                                Data.ClientsInfo.Remove(clientInfo);
                                Console.WriteLine($"Найден плохой клиент {clientInfo.Nick}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                    }
                }
            }*/

            static void ListenClients() //Поиск клиентов(Создание потоков с клиентами)
            {
                while (true)
                {  
                    Task.Delay(10).Wait();//Задержка

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
                {
                    try
                    {
                        Task.Delay(10).Wait();

                        int i = client.Client.Receive(buffer);
                        string message = Encoding.UTF8.GetString(buffer, 0, i);
                        
                        if(message != "")
                        {
                            char ch = ':'; //Разделяющий символ
                            string command = message.Substring(1, (message.IndexOf(ch) - 1)); //Команда 
                            string[] arguments = message.Substring(message.IndexOf(ch) + 1).Split(new char[] { ch }); //Массив аргументов

                            //string[] spiting = message.Split();
                            
                            try{ //Обращение к командам
                                #region Как команда от клиенте?
                                
                                Console.WriteLine("\n" + "Команда: " + command + " \n ");
                                Console.WriteLine("Аргументы:\n----------");
                                foreach (string s in arguments)
                                {
                                    Console.WriteLine(s);
                                }
                                Console.WriteLine("----------");
                                
                                #endregion
                                
                                Commands ComandClass = new Commands();
                                ComandClass.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(ComandClass, new object[] {client, arguments});
                            }
                            catch(Exception ex)
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
}
