using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {

        public void LOG(object obj, string[] argumets) // %LOG:email:pass
        {
            var email = argumets[0];
            var password = argumets[1];

            if (Database.CheckEmail(email))
            {
                if (Database.CheckPassword(email, password))
                {
                    var info = Database.GetClientInfo(email);
                    Function.SendClientMessage(obj, $"%LOGOOD:{info.ID}:{info.EMAIL}:{info.NICK}:{info.COEF}:{info.LEVEL}");
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