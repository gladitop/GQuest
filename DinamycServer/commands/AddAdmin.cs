using Org.BouncyCastle.Utilities.Net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DinamycServer.commands
{
    public partial class Commands
    {
        /* Добавить админа (может только админ)
           %ФADDAD:login */
        public void ADDAD(object client, string[] argumets)
        {
            string login = argumets[0];

            if (Function.CheckAdmin((TcpClient)client))
            {
                if (!Database.CheckLoginAdmin(login))
                {
                    var info = new Data.AdminInfo(login);
                    Database.AddAdminAccount(info.Login, info.SHAPassword);
                    Function.WriteConsole($"Add new admin in BD: {Function.GetSocketIP(client)}:{login}", ConsoleColor.Yellow);
                }
                else
                {
                    Function.SendClientMessage(client, "%BREG");
                }
            }
            else
            {
                Function.WriteConsole($"WARING: HACKER! {Function.GetSocketIP((TcpClient)client)}");
            }
        }
    }
}
