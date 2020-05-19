using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static public TcpListener server { get; set; }
        static public Database database { get; set; }

        static void Main(string[] args)
        {
            Console.Title = "Сервер";
            Function.WriteLine("Это сервер для говно теста", ConsoleColor.Green);
            Function.WriteLine("Загрузка сервера...", ConsoleColor.Yellow);

            database = new Database();
            Thread thread1 = new Thread(new ThreadStart(ClearBadClient));
            thread1.Start();
            server = new TcpListener(IPAddress.Any, Data.Port);
            server.Start();
            Thread thread = new Thread(new ThreadStart(ListenClients));
            thread.Start();

            Function.WriteLine("Сервер работает!", ConsoleColor.Green);

            Console.ReadLine();
            string answer = "";
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
                    {
                        try
                        {
                            Ping ping = new Ping();
                            PingReply status = ping.Send(clientInfo.IP.Address);
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
            }

            static void ClientManager(object obj) //Прослушка
            {
                Data.ClientInfoOnly clientInfo = (Data.ClientInfoOnly) obj;
                byte[] buffer = new byte[1024];

                while (true)
                {
                    try
                    {
                        Task.Delay(10).Wait();

                        int i = clientInfo.Socket.Client.Receive(buffer);
                        string answer = Encoding.UTF8.GetString(buffer, 0, i);

                        if (answer.Contains("%PCOUNT"))
                        {
                            //%PCOUNT:{Client.id}:{points}

                            Match regex = Regex.Match(answer, "%PCOUNT:(.*):(.*)");
                            long id = Convert.ToInt64(regex.Groups[1].Value);
                            long point = Convert.ToInt64(regex.Groups[2].Value);

                            Data.InfoPoint infoPoint = new Data.InfoPoint(id, point);
                            database.AddPoint(infoPoint);
                        }
                    }
                    catch (Exception ex)
                    {
                        Function.WriteLine($"Ошибка: {ex.Message}", ConsoleColor.Red);
                    }
                }
            }

            #endregion

            #region Подключение клиента

            static void ClientLog(object obj) //Чтобы клиент вошёл в систему
            {
                TcpClient client = (TcpClient) obj;
                byte[] buffer = new byte[1024];

                while (true)
                {
                    try
                    {
                        Task.Delay(10).Wait();

                        int i = client.Client.Receive(buffer);
                        string answer = Encoding.UTF8.GetString(buffer, 0, i);

                        if (answer.Contains("%REG")) //Регистрация
                        {
                            //REG:{email}:{pass}:{clientName}
                            Console.WriteLine(answer);
                            Match regex = Regex.Match(answer, "%REG:(.):(.):(.*)");
                            string email = regex.Groups[1].Value;
                            string password = regex.Groups[2].Value;
                            string nick = regex.Groups[3].Value;
                            //TODO: Сделать проверку почты

                            //Добавление аккаунта
                            try
                            {
                                database.AddAccount(email, password, nick);
                            }
                            catch
                            {
                                client.Client.Send(Encoding.UTF8.GetBytes("%BREG"));
                                return;
                            }

                            Console.WriteLine("GoodRegistration");
                            client.Client.Send(Encoding.UTF8.GetBytes("%REGOOD"));
                            //Добавление в систему

                            Data.ClientInfoOnly clientInfo = new Data.ClientInfoOnly(client, email, password, nick);
                            Thread thread = new Thread(new ParameterizedThreadStart(ClientManager));
                            thread.Start(clientInfo);
                            return;
                        }
                        else if (answer.Contains("%LOG"))
                        {
                            //%LOG:{email}:{pass}
                            Console.WriteLine("1");
                            Match regex = Regex.Match(answer, "%LOG:(.*):(.*):");
                            string email = regex.Groups[1].Value;
                            string password = regex.Groups[2].Value;
                            Console.WriteLine(email);
                            //Проверка почты

                            if (!database.CheckEmail(email))
                            {
                                Console.WriteLine("1");
                                //Проверка пароля

                                if (!database.CheckPassword(password, email))
                                {
                                    //Получение инфы и вход в систему
                                    Console.WriteLine("GOODCHECKPASSWORD");
                                    Data.ClientInfoOffile info = database.GetClientInfo(email);
                                    //POINT МОЖЕТ БЫТЬ ПУСТЫМ!!! 

                                    if (info.Point == null)
                                    {
                                        info.Point = 0;
                                    }

                                    client.Client.Send(
                                        Encoding.UTF8.GetBytes($"%LOGOD:{info.ID}:{info.Nick}:{info.Point}"));
                                    Console.WriteLine($"%LOGOD:{info.ID}:{info.Nick}:{info.Point}");
                                    Data.ClientInfoOnly clientInfo = new Data.ClientInfoOnly(client, info.Email,
                                        info.Password,
                                        info.Nick);
                                    Data.ClientsInfo.Add(clientInfo);
                                    Thread thread = new Thread(new ParameterizedThreadStart(ClientManager));
                                    thread.Start(clientInfo);
                                    Console.WriteLine("1");
                                    return;
                                }
                                else
                                {
                                    goto badling;
                                }
                            }
                            else
                            {
                                goto badling;
                            }

                            badling: ; //LOL
                            client.Client.Send(Encoding.UTF8.GetBytes("%BLOG"));
                            Console.WriteLine("2");
                        }
                    }
                    catch (Exception ex)
                    {
                        Function.WriteLine($"Ошибка: {ex.Message}", ConsoleColor.Red);
                    }
                }
            }

            static void ListenClients() //Поиск клиентов
            {
                while (true)
                {
                    Task.Delay(10).Wait();
                    TcpClient client = server.AcceptTcpClient();
                    Thread thread = new Thread(new ParameterizedThreadStart(ClientLog));
                    thread.Start(client);
                }
            }

            #endregion
        }
    }
}