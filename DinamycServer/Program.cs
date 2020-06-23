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

            Console.WriteLine("Start server...");

            /*
            if (!Directory.Exists("LOG"))
                Directory.CreateDirectory("LOG");
            */

            Data.Logger = new StreamWriter("LOG.log", true);
            Data.Logger.AutoFlush = true;

            server = new TcpListener(IPAddress.Any, Data.Port);
            server.Start();
            var thread = new Thread(ListenClients);
            thread.Start();

            Function.WriteColorText("Done server!", ConsoleColor.Green);

            var answer = "";
            while (true)
            {
                answer = Console.ReadLine();

                switch (answer.ToLower())
                {
                    case "stop": //Остановка сервера (После этого ctr + c)
                        Function.WriteColorText("off server...");

                        if (Data.TpClient.Count == 0)
                            goto Link;

                        foreach (var client in Data.TpClient)
                            try
                            {
                                client.Client.Close();
                                Function.WriteColorText("off client");
                            }
                            catch (Exception e)
                            {
                                Function.WriteColorText($"error: {e.Message}");
                            }

                        server.Stop();
                        Link:
                        Function.WriteColorText("Done off server!", ConsoleColor.Green);
                        Data.Logger.Close();
                        Environment.Exit(0);
                        break;
                    case "help": //хелб
                        Function.WriteColorText("stop - this is stop", ConsoleColor.Yellow);
                        break;
                }
            }

            #endregion

            #region Клиенты

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

                Function.WriteColorText("new connect!");
                Data.TpClient.Add(client);
                end:
                while (true)
                {
                    var message = "";

                    try
                    {
                        Task.Delay(10).Wait();

                        var i = client.Client.Receive(buffer);
                        if (i == 1) goto end;

                        message = Encoding.UTF8.GetString(buffer, 0, i);
                    }
                    catch (Exception ex)
                    {
                        Function.CheckEmptyClients(client);
                        Function.WriteColorText($"ERROR_1: {ex}");
                        return; //Чтобы памяти было много)
                    }

                    if (message != "")
                    {
                        Function.WriteColorText(message);

                        string command;
                        string[] arguments;

                        try
                        {
                            var ch = ':'; //Разделяющий символ
                            command = message.Substring(1, message.IndexOf(ch) - 1); //Команда 
                            arguments = message.Substring(message.IndexOf(ch) + 1)
                                .Split(new[] {ch}); //Массив аргументов
                        }
                        catch
                        {
                            Function.WriteColorText($"ERROR_2\n Incoming message: {message}", ConsoleColor.Yellow);
                            goto end;
                        }

                        try
                        {
                            var ComandClass = new Commands();
                            ComandClass.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.NonPublic)
                                .Invoke(ComandClass, new object[] {client, arguments});
                        }
                        catch (Exception ex)
                        {
                            #region ERROR_3

                            Function.WriteColorText("ERROR_3: " + ex, ConsoleColor.Yellow);
                            try
                            {
                                var needArg = (string[]) typeof(Commands).GetField($"arg{command}").GetValue(null);

                                Function.WriteColorText("Expected argument | Incoming\n----------", ConsoleColor.White);
                                for (var j = 0; j < needArg.Length; j++)
                                    try
                                    {
                                        if (arguments[j] == "" || arguments[j] == " " || arguments[j] == null)
                                            arguments[j] = "null";
                                        Function.WriteColorText($"{needArg[j]} | {arguments[j]}", ConsoleColor.White);
                                    }
                                    catch
                                    {
                                        Function.WriteColorText($"{needArg[j]} | null", ConsoleColor.White);
                                    }

                                Function.WriteColorText("----------", ConsoleColor.Yellow);
                            }
                            catch
                            {
                                Function.WriteColorText(
                                    "Could not find the list of required arguments!\nYou must add string [] {required arguments}!",
                                    ConsoleColor.DarkGray);
                            }

                            #endregion

                            goto end;
                        }
                    }
                }
            }

            #endregion
        }
    }
}