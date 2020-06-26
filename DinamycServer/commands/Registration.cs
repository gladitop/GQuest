using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        public void REG(object client, string[] argumets) // %REG:email:pass:nick
        {
            try
            {
                string email = argumets[0];
                string password = argumets[1];
                string nick = argumets[2];

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
                Function.WriteConsole($"REG:{e.Message}", ConsoleColor.Red);
            }
        }
    }
}