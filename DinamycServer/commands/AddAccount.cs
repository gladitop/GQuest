using System;
using System.Net;

namespace DinamycServer
{
    public partial class Commands
    {
        private void ADAC(string email, string password, string nick, string coef, string level, string levelcomplete,
            bool isAdmin, bool ofAdmin, IPAddress ofIp) //Добавить аккаунт (только админ)
        {
            if (!ofAdmin) //Потом придём в гости)
            {
                Function.WriteColorText($"A hacker was found {ofIp}! AddAccount", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(coef))
            {
            }
        }
    }
}