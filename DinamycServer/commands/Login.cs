using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        public static string[] argLOG = {"email", "password"}; // строка-подсказка с необходимыми аргументами

        private void LOG(TcpClient client, string[] argumets) // %LOG:email:pass
        {
            var email = argumets[0];
            var password = argumets[1];

            try
            {
                if (Database.CheckEmail(email))
                {
                    if (Database.CheckPassword(email, password))
                    {
                        var info = Database.GetClientInfo(email);

                        Function.SendClientMessage(client,
                            $"%LOGOOD:{info.ID}:{info.Email}:{info.Nick}:{info.Coef}:{info.Level}");
                    }
                    else
                    {
                        Function.SendClientMessage(client, "%BLOG:");
                    }
                }
                else
                {
                    Function.SendClientMessage(client, "%BLOG:");
                }
            }
            catch (Exception e)
            {
                Function.WriteColorText($"LOG:{e.Message}", ConsoleColor.Red);
            }
        }
    }
}