using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        private void MSG(TcpClient client, string[] argumets) // %MSG:nick:message
        {
            var nick = argumets[1];
            var msg = argumets[2];

            //MES:{NICK}:{MESS}
            Function.SendMessage(msg, nick);
            Console.WriteLine("Message= " + msg);
        }
    }
}