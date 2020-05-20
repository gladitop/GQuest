using System;
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
        public static Database database { get; set; }

        static void Main(string[] args)
        {            
            database = new Database();
            var thread1 = new Thread(ClearBadClient);
            thread1.Start();
            server = new TcpListener(IPAddress.Any, Data.Port);
            server.Start();
            var thread = new Thread(ListenClients);
            thread.Start();

            Console.WriteLine("Сервер работает!");

            var answer = "";
        
            static void ClearBadClient() //Очистка 'Плохих' клиентов
            {
                while (true)
                {
                    Task.Delay(1000).Wait();

                    foreach (var clientInfo in Data.ClientsInfo)
                        try
                        {
                            var ping = new Ping();
                            var status = ping.Send(clientInfo.IP.Address);
                            if (status.Status != IPStatus.Success)
                            {
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

            static void ListenClients() //Поиск клиентов(Создание потока для общение с клиентом)
            {
                while (true)
                {  
                    Task.Delay(10).Wait();//Задержка

                    var client = server.AcceptTcpClient();
                    var thread = new Thread(ClientLog);
                    thread.Start(client);         
                }
            }

            static void ClientLog(object obj) //Чтобы клиент вошёл в систему
            {
                var client = (TcpClient) obj;
                var buffer = new byte[1024];

                while (true)
                    try
                    {
                        Task.Delay(10).Wait();

                        var i = client.Client.Receive(buffer);
                        var message = Encoding.UTF8.GetString(buffer, 0, i);
                        
                  
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
            }
        }   
    }
}
