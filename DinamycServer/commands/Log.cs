using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace DinamycServer
{
    public partial class Commands
    {
        void LOG(TcpClient client, string[] argumets) // %LOG:email:pass
        {
            string email = null;
            string passworld = null;
            try{
                
                email = argumets[0];
                passworld = argumets[1];
            }
            catch(Exception ex){Console.WriteLine("\nОшибка при проверке аргументов:\n----------\n" + ex + "\n----------");}

            if (Database.LogAccount(email, passworld))
            {
                Function.SendClientMessage(client, "%LOGOOD");
            }
            else
            {
                Function.SendClientMessage(client, "%BLOG");
            }
        }
    }
}