using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        private void MSG(TcpClient client, string[] argumets) // %MSG:nick:message
        {
            var nick = "nick";
            var msg = "";
            try
            {
                foreach (var str in argumets) msg += $"{str} ";
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nОшибка при проверке аргументов:\n----------\n" + ex + "\n----------");
            }

            msg = msg.Substring(5);
            Function.SendClientMessage(client, $"%MES:{nick}:{msg}");
        }
    }
}