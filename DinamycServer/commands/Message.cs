using System;
using System.Net.Sockets;
using System.Text;

namespace DinamycServer
{
    public partial class Commands
    {
        private void MSG(TcpClient client, string[] argumets) // %MSG:nick:message
        {
            var nick = "nick";//TODO:Сам сделаешь!
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

            //1 Вариант

            Data.NetworkStream.Write(Encoding.UTF8.GetBytes(msg));

            //2 Вариант
        }
    }
}