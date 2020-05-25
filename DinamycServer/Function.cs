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
                if (client != null)
                    client.Client.Send(Encoding.UTF8.GetBytes(message));
            }
            catch
            {
                Function.WriteColorText("ERRMESS!", ConsoleColor.Red);
                DeleteClient(client);
            }
        }

        #region Удаление клиента 
        //TODO: Переделывай, и проверяй там где идёт входящие сообщение Program строка 73 и сделать цикл с удаление каждую минуту

        public static void DeleteClient(Data.ClientInfo clientInfo)
        {
            clientInfo.Socket.Close();
            Data.ClientsInfo.Remove(clientInfo);
            WriteColorText($"Удалён клиент: {clientInfo.Email}", ConsoleColor.Green);
        }
        
        public static void DeleteClient(TcpClient client)
        {
            client.Close();
            foreach (var clientInfo in Data.ClientsInfo)
            {
                if (clientInfo.Socket == client)
                {
                    Data.ClientsInfo.Remove(clientInfo);
                    break;
                }
            }
            
            WriteColorText($"Удалён клиент: {(IPEndPoint)client.Client.LocalEndPoint}", ConsoleColor.Green);
        }
        
        #endregion

        public static void WriteColorText(string text, ConsoleColor color)//Хватит изменить название!
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void SendMessage(string message, string nick) //Отправить всем сообщение
        {
            foreach (var clientInfo in Data.ClientsInfo)
            {
                try
                {
                    if (clientInfo.Socket != null)
                    {
                        //MES:{NICK}:{MESS}
                        SendClientMessage(clientInfo.Socket, $"%MES:{nick}:{message}");
                    }
                }
                catch
                {
                    Function.WriteColorText("ERRSendMessage!", ConsoleColor.Red);
                    Function.DeleteClient(clientInfo);
                }
            }
        }
    }
}