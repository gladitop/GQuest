using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
       private void MSG(TcpClient client, string[] argumets) // %MSG:nick:message
        {
            string nick = "nick";
            string msg = "";
            try
            {
                foreach(string str in argumets)
                {
                    msg += $"{str} ";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nОшибка при проверке аргументов:\n----------\n" + ex + "\n----------");
            }
            msg = msg.Substring(5);//TODO
            //Для Антона TODO:Пиши что надо сделать в TODO!

            Function.SendClientMessage(client, $"%MES:{nick}:{msg}");
        }
    }
}