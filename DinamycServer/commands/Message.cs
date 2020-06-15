using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        private void MSG(TcpClient client, string[] argumets) // %MSG:nick:message //отправка сообщения всем
        {
            try
            {
                var nick = argumets[0];
                var msg = argumets[1];

                Function.WriteColorText("Goood");
                Function.WriteColorText("help");
                Function.SendMessage(nick, msg);
                Function.WriteColorText("Message= " + msg);
            }
            catch(Exception e)
            {
                Function.WriteColorText($"MSG:{e.Message}", ConsoleColor.Red);

            }
        }
    }
}