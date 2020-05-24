using System;
using System.Net.Sockets;
using System.Text;

namespace DinamycServer
{
    public partial class Commands
    {
        private void MSG(TcpClient client, string[] argumets) // %MSG:nick:message
        {
            var msg = argumets[1];

            Function.SendMessage(msg);
            Console.WriteLine("Message= " + msg);
        }
    }
}