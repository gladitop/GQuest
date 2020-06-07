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
            #region Запуск сервера + консольные команды

            Console.WriteLine("Запуск сервера...");
            server = new TcpListener(IPAddress.Any, Data.Port);
            server.Start();
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

                Console.WriteLine("новое подключение");
                Data.TpClient.Add(client);
                end:
                while (true)
                    try
                    {
                        Task.Delay(10).Wait();

                        var i = client.Client.Receive(buffer);
                        var message = Encoding.UTF8.GetString(buffer, 0, i);

                        if (message != "")
                        {
                            string command;
                            string[] arguments;

                            try
                            {
                                var ch = ':'; //Разделяющий символ
                                command = message.Substring(1, message.IndexOf(ch) - 1); //Команда 
                                arguments = message.Substring(message.IndexOf(ch) + 1).Split(new[] {ch}); //Массив аргументов
                            }
                            catch
                            {
                                Function.WriteColorText($"ERROR_2\nВходящее сообщение: {message}",ConsoleColor.Yellow);
                               goto end;
                            }

                            try
                            {       
                                var ComandClass = new Commands();                     
                                ComandClass.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(ComandClass, new object[] {client, arguments});
                            }
                            catch
                            {
                                #region ERROR_3
                                
                                Function.WriteColorText("ERROR_3", ConsoleColor.Yellow);
                                try
                                {
                                    string[] needArg = (string[])typeof(Commands).GetField($"arg{command}").GetValue(null);

                                    Function.WriteColorText("Ожидаемый аргумент | Входящий\n----------", ConsoleColor.White);
                                    for (int j = 0; j < needArg.Length; j++)
                                    {                        
                                        try{if(arguments[j] == "" || arguments[j] == " " || arguments[j] == null) arguments[j] = "пусто";
                                        Function.WriteColorText($"{needArg[j]} | {arguments[j]}", ConsoleColor.White);}
                                        catch{Function.WriteColorText($"{needArg[j]} | пусто", ConsoleColor.White);}           
                                    }
                                    Function.WriteColorText("----------", ConsoleColor.Yellow);
                                }
                                catch{Function.WriteColorText("Не удалось найти список нужных аргументов!\nНеобходимо добавить string[]{необходимые аргументы}!", ConsoleColor.DarkGray); }
                                
                                #endregion
                                
                                goto end;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR_1: {ex}");
                        goto end;
                    }
            }
            
            #endregion

        }
    }
}
