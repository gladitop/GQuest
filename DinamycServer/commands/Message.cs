using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        public void MSG(TcpClient client, string[] argumets) // %MSG:nick:message //отправка сообщения всем //TODO: сделаем позже
        {
            try
            {
                var nick = argumets[0];
                var msg = argumets[1];

                Function.WriteConsole("Goood");
                Function.WriteConsole("help");
                //Function.SendMessage(nick, msg);
                Function.WriteConsole("Message= " + msg);
            }
            catch (Exception e)
            {
                Function.WriteConsole($"MSG:{e.Message}", ConsoleColor.Red);
            }
        }
    }
}