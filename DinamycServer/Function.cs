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
            if(CheckingClient != null)
            {
                if(!CheckingClient.Connected)
                {
                    CheckingClient.Close();
                    Data.TpClient.Remove(CheckingClient);
                    WriteColorText("Удалён клиент", ConsoleColor.Yellow);              
                }             
            }
            else
            { 
                int i = 0;
                Console.WriteLine(Data.TpClient.Count);  
                foreach (var client in Data.TpClient)
                {
                    if(!client.Connected)
                    {
                        Console.WriteLine("нет");
                        i++;
                        client.Close();
                        Data.TpClient.Remove(client);           
                    }          
                }
                WriteColorText($"Произведенна очистка клиетов, очищенно:{i}", ConsoleColor.Yellow);   
                Console.WriteLine(Data.TpClient.Count);         
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
            CheckEmptyClients(null); //очистка пустых клинтов
            foreach (var client in Data.TpClient)
            {
                SendClientMessage(client, $"%MES:{nick}:{message}");               
                
            }
        }
    }
}