using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
       private void REG(TcpClient client, string[] argumets) // %REG:email:pass:nick
        {
            string email = null;
            string password = null;
            string nick = null;
            try
            {
                email = argumets[0];
                password = argumets[1];
                nick = argumets[2];
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nОшибка при проверке аргументов:\n----------\n" + ex + "\n----------");
            }

            if (!Database.CheckEmail(email))
            {
                Database.AddAccount(email, password, nick);
                Function.SendClientMessage(client, "%REGOOD");
            }
            else Function.SendClientMessage(client, "%BREG");
        }
    }
}