using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        private void MSG(TcpClient client, string[] argumets) // %MSG:nick:message
        {
            try
            {
                var nick = argumets[1];
                var msg = argumets[2];

                //MES:{NICK}:{MESS}
                Function.SendMessage(msg, nick);
                Console.WriteLine("Message= " + msg);
            }
            catch(Exception e)
            {
                Function.WriteColorText($"MSG:{e.Message}", ConsoleColor.Red);
                Function.DeleteClient(client);
            }
        }
    }
}