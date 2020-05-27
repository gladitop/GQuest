using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DinamycServer
{
    public static class Function //Функции
    {
        public static void SendClientMessage(TcpClient client, string message) //Отравить клиенту сообщение
        {
            try
            {
                client.Client.Send(Encoding.UTF8.GetBytes(message));
            }
            catch
            {
                CheckEmptyClients(client);
                Function.WriteColorText("ERRMESS!", ConsoleColor.Red);
            }
        }

        public static void CheckEmptyClients(TcpClient CheckingClient)//Поиск пустых клиентов и их удаление
        {
            //public static bool IsSocketStillConnected(Socket socket)
            if(CheckingClient != null)
            {
                check(CheckingClient);
            }
            else
            {
                Console.WriteLine(Data.TpClient.Count); 
                Console.WriteLine("6");
                foreach(var ChClients in Data.TpClient)
                {
                    Console.WriteLine("7");
                    check(ChClients);
                }                
                WriteColorText($"Произведенна очистка клиетов", ConsoleColor.Yellow);   
                Console.WriteLine(Data.TpClient.Count); 
            }
            void check(TcpClient cl)
            {
                Console.WriteLine("8");
                try
                {
                    Console.WriteLine("9");              
                    cl.Client.Send(new byte[1]); //Это работает, незнаю как, главное, что работает!
                }
                catch
                {
                    Console.WriteLine("10");
                    Data.TpClient.Remove(cl);
                    cl.Close();
                    WriteColorText("Удалён клиент", ConsoleColor.Yellow); 
                }
            }
        }

        public static void WriteColorText(string text, ConsoleColor color)//Хватит изменить название!
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void SendMessage(string nick, string message) //Отправить всем сообщение
        {
            foreach (var client in Data.TpClient)
            {
                try
                {
                    SendClientMessage(client, $"%MES:{nick}:{message}"); 
                }     
                catch
                {
                    CheckEmptyClients(client);
                }      
                
            }
        }
    }
}