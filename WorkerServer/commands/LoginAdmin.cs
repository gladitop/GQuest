using System;

namespace WorkerServer
{
    public partial class Commands
    {
        public void LOGA(object obj, string[] argumets) // %LOGA:login:pass
        {
            var login = argumets[0];
            var password = argumets[1]; //TODO:Какой-то херня

            try
            {
                if (Database.CheckDataAdmin(login, password))
                {
                    Function.SendClientMessage(obj, "%GODLOG:");
                    //TODO:Придумать систему для админа
                }
                else
                {
                    Function.SendClientMessage(obj, "%BLOG:");
                    Function.WriteConsole("BLOG:ADMIN", ConsoleColor.Red);
                }
            }
            catch
            {
                Function.WriteConsole("ERR:LODAADMIN", ConsoleColor.Red);
            }
        }
    }
}