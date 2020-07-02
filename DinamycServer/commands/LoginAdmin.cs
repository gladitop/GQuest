using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        public void LOGA(object obj, string[] argumets) // %LOGA:login:pass
        {
            var login = argumets[0];
            var password = argumets[1];

            if (Database.CheckLoginAdmin(login))
            {
                if (Database.CheckPassword(login, password))
                {
                    Function.SendClientMessage(obj,$"%LOGOOD");
                    Function.WriteConsole($"LOGOODP {Function.GetSocketIP((TcpClient)obj)}");

                    foreach (var i in Data.Clients)
                    {
                        if (i.TpClient == obj)
                        {
                            i.Admin = true;
                            break;
                        }
                    }
                }
                else
                {
                    Function.SendClientMessage(obj, "%BLOG:");
                }
            }
            else
            {
                Function.SendClientMessage(obj, "%BLOG:");
            }
        }
    }
}