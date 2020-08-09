using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerServer
{
    public class Worker : BackgroundService //Сам наш демон
    {
        #region Демон

        public Worker(ILogger<Worker> logger) //Запуск демона
        {
            Data._logger = logger;

            Data._logger.LogInformation("Starting service...", DateTimeOffset.Now);

            StartServer();

            Data._logger.LogInformation("Service done!", DateTimeOffset.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) //Просто лог
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Data._logger.LogDebug("Working: {time}", DateTimeOffset.Now);
                await Task.Delay(3000, stoppingToken); //Будем писать лог каждый 3 секунды
            }
        }

        #endregion

        # region Сервер

        public static TcpListener server { get; set; }

        public static void StartServer() //Запуск сервера
        {
            #region Запуск сервера + консольные команды

            //CreateHostBuilder(args).Build().Run(); //Билд демона

            server = new TcpListener(IPAddress.Any, Data.Port);
            server.Start();
            var threadListenClient = new Thread(ListenClients);
            threadListenClient.Start();

            Function.WriteConsole("Done server!", ConsoleColor.Green);

            #endregion


            var answer = "";
            while (true)
            {
                answer = Console.ReadLine();

                switch (answer.ToLower())
                {
                    case "stop": //Остановка сервера (После этого ctr + c)
                        Function.WriteConsole("off server...", ConsoleColor.Yellow);

                        Data.ServerStart = false;
                        threadListenClient.Join(); //Навсякий случай (из-за другого всё ломается)
                        start:
                        if (server.Pending())
                        {
                            Function.WriteConsole("Waiting...", ConsoleColor.Yellow);
                            Task.Delay(100).Wait();
                            goto start;
                        }

                        server.Stop();
                        Function.WriteConsole("Done off server!", ConsoleColor.Green);
                        Environment.Exit(0);
                        break;
                    default:
                        Function.WriteConsole("No command!", ConsoleColor.Red);
                        break;
                }
            }
        }

        #region Клиент

        private static void ListenClients() //Поиск клиентов(Создание потоков с клиентами)
        {
            while (Data.ServerStart)
                try
                {
                    Task.Delay(10).Wait();

                    if (!server.Pending())
                    {
                        Task.Delay(500).Wait();
                        continue;
                    }


                    var client = server.AcceptTcpClient(); //клиент
                    var thread = new Thread(ClientLog); //поток

                    Data.Clients.Add(new Data.ThreadClient(client, thread)); //заносим в массив
                    thread.Start(new Data.ThreadClient(client, thread));
                }
                catch
                {
                    Function.WriteConsole("ERROR: Add new connect", ConsoleColor.Red);
                }
        }

        private static void ClientLog(object obj) //Поток клиента
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

                if (!message.Contains("%")) goto end; //проверка на пустое сообщение
                message.Substring(message.IndexOf('%'));
                Function.WriteConsole(message);

                var ch = ':'; //Разделяющий символ
                var command = message.Substring(1, message.IndexOf(ch) - 1); //Команда 
                var arguments = message.Substring(message.IndexOf(ch) + 1).Split(new[] {ch}); //Массив аргументов

                var ComandClass = new Commands();

                foreach (var method in ComandClass.GetType().GetTypeInfo().GetMethods())
                    if (method.Name == command)
                    {
                        ComandClass.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.Public)
                            .Invoke(ComandClass, new[] {obj, arguments});
                        Function.WriteConsole(message, ConsoleColor.Green);
                        goto end;
                    }

                Function.WriteConsole($"Error_1: команда:{command}", ConsoleColor.Red);
            }
        }

        #endregion
    }

    # endregion
}