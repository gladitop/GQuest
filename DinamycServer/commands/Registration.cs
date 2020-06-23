using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        private void REG(TcpClient client, string[] argumets) // %REG:email:pass:nick
        {
            try
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
                    Function.WriteColorText("\nError checking arguments:\n----------\n" + ex + "\n----------");
                }

                if (!Database.CheckEmail(email))
                {
                    Database.AddAccount(email, password, nick);
                    Function.SendClientMessage(client, "%REGOOD:");
                }
                else
                {
                    Function.SendClientMessage(client, "%BREG:");
                }
            }
            catch (Exception e)
            {
                Function.WriteColorText($"REG:{e.Message}", ConsoleColor.Red);
            }
        }
    }
}