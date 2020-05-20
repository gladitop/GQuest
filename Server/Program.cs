using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        public static TcpListener server { get; set; }
        public static Database database { get; set; }

        private static void Main(string[] args)
        {
            Console.Title = "Сервер";
            Function.WriteLine("Это сервер для говно теста", ConsoleColor.Green);
            Function.WriteLine("Загрузка сервера...", ConsoleColor.Yellow);

            database = new Database();
            var thread1 = new Thread(ClearBadClient);
            thread1.Start();
            server = new TcpListener(IPAddress.Any, Data.Port);
            server.Start();
            var thread = new Thread(ListenClients);
            thread.Start();

            Function.WriteLine("Сервер работает!", ConsoleColor.Green);

            var answer = "";
            while (true)
            {
                answer = Console.ReadLine();

                switch (answer)
                {
                    case "exit":
                        Function.WriteLine("Отключение сервера...", ConsoleColor.Yellow);
                        server.Stop();
                        break;
                }
            }

            #region Управление клиентом

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
                                Function.WriteLine($"Найден плохой клиент {clientInfo.Nick}", ConsoleColor.Green);
                            }
                        }
                        catch (Exception ex)
                        {
                            Function.WriteLine($"Ошибка: {ex.Message}", ConsoleColor.Red);
                        }
                }
            }

            static void ClientManager(object obj) //Прослушка
            {
                var clientInfo = (Data.ClientInfoOnly) obj;
                var buffer = new byte[1024];

                while (true)
                    try
                    {
                        Task.Delay(10).Wait();

                        var i = clientInfo.Socket.Client.Receive(buffer);
                        var answer = Encoding.UTF8.GetString(buffer, 0, i);
                        if (answer.Contains("%PCOUNT"))
                        {
                            //%PCOUNT:{Client.id}:{points}

                            var regex = Regex.Match(answer, "%PCOUNT:(.*):(.*)");
                            var id = Convert.ToInt64(regex.Groups[1].Value);
                            var point = Convert.ToInt64(regex.Groups[2].Value);

                            var infoPoint = new Data.InfoPoint(id, point);
                            database.AddPoint(infoPoint);
                            return;
                        }

                        if (answer.Contains("")) //Потом добавь!
                        {
                            var infos = database.GetScore();

                            foreach (var info in infos)
                                try
                                {
                                    clientInfo.Socket.Client.Send(
                                        Encoding.UTF8.GetBytes($"{info.Email}:{info.Point}")); //И тут тоже!
                                }
                                catch (Exception e)
                                {
                                    Function.WriteLine("Ошибка " + e.Message, ConsoleColor.Red);
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        Function.WriteLine($"Ошибка: {ex.Message}", ConsoleColor.Red);
                    }
            }

            #endregion

            #region Подключение клиента

            static void ClientLog(object obj) //Чтобы клиент вошёл в систему
            {
                var client = (TcpClient) obj;
                var buffer = new byte[1024];

                while (true)
                    try
                    {
                        Task.Delay(10).Wait();

                        var i = client.Client.Receive(buffer);
                        var answer = Encoding.UTF8.GetString(buffer, 0, i);

                        if (answer.Contains("%REG")) //Регистрация
                        {
                            Console.WriteLine(
                                $"Регистрация пользователя: {answer}"); //REG:{email}:{pass}:{clientName}                           
                            var regex = Regex.Match(answer, "%REG:(.*):(.*):(.*)");
                            var email = regex.Groups[1].Value;
                            var password = regex.Groups[2].Value;
                            var nick = regex.Groups[3].Value;

                            if (database.CheckEmail(email))
                            {
                                Console.WriteLine("Проверка почты: %BREG");
                                client.Client.Send(Encoding.UTF8.GetBytes("%BREG"));
                            }
                            else
                            {
                                database.AddAccount(email, password, nick);
                                Console.WriteLine("Проверка почты: %REGOOD");
                                client.Client.Send(Encoding.UTF8.GetBytes("%REGOOD"));
                                //Добавление в систему

                                var clientInfo = new Data.ClientInfoOnly(client, email, password, nick);
                                var thread = new Thread(ClientManager);
                                thread.Start(clientInfo);
                            }

                            return;
                        }

                        if (answer.Contains("%LOG"))
                        {
                            Console.WriteLine($"Логин пользователя: {answer}"); //%LOG:{email}:{pass}
                            var regex = Regex.Match(answer, "%LOG:(.*):(.*)");
                            var email = regex.Groups[1].Value;
                            var password = regex.Groups[2].Value;

                            if (database.CheckEmail(email)) //Проверка почты
                            {
                                Console.WriteLine($"Логин: Проверка почты успешно: {email}");

                                if (database.CheckPassword(email, password)) //Проверка пароля
                                {
                                    Console.WriteLine($"Логин: Проверка пароля успешно: {password}");
                                    var info = database.GetClientInfo(email);

                                    if (info.Point == null) //POINT НЕ МОЖЕТ БЫТЬ ПУСТЫМ!!! 
                                        info.Point = 0;

                                    client.Client.Send(
                                        Encoding.UTF8.GetBytes($"%LOGOD:{info.ID}:{info.Nick}:{info.Point}"));
                                    var clientInfo = new Data.ClientInfoOnly(client, info.Email,
                                        info.Password,
                                        info.Nick);
                                    Data.ClientsInfo.Add(clientInfo);
                                    var thread = new Thread(ClientManager);
                                    thread.Start(clientInfo);
                                    return;
                                }
                            }

                            client.Client.Send(Encoding.UTF8.GetBytes("%BLOG"));
                            Console.WriteLine("Проверка логина: %BLOG");
                        }
                    }
                    catch (Exception ex)
                    {
                        Function.WriteLine($"Ошибка: {ex.Message}", ConsoleColor.Red);
                    }
            }

            static void ListenClients() //Поиск клиентов
            {
                while (true)
                {
                    Task.Delay(10).Wait();
                    var client = server.AcceptTcpClient();
                    var thread = new Thread(ClientLog);
                    thread.Start(client);
                }
            }

            #endregion
        }
    }
}
