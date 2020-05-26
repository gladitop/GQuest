using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        private void LOG(TcpClient client, string[] argumets) // %LOG:email:pass
        {
            string email = null;
            string password = null;
            try
            {
                email = argumets[0];
                password = argumets[1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nОшибка при проверке аргументов:\n----------\n" + ex + "\n----------");
            }

            try
            {
                if (Database.CheckEmail(email))
                {
                    if (Database.CheckPassword(email, password))
                    {
                        var info = Database.GetClientInfo(email);
                        Function.SendClientMessage(client, $"%LOGOOD:{info.Email}:{info.ID}:{info.Nick}:{info.Point}");
                        
                        /*
                        //Добавление клиента
                        Data.ClientInfo infoClient = new Data.ClientInfo();
                        info.ID = Data.AddInfo.Count + 1;
                        info.Email = info.Email;
                        Data.AddInfo.Add(infoClient);
                        */

                        var clientInfo = Database.GetClientInfo(email);
                        Data.ClientsInfo.Add(clientInfo);
                    }
                    else
                    {
                        Function.SendClientMessage(client, "%BLOG");
                    }
                }
                else
                {
                    Function.SendClientMessage(client, "%BLOG");
                }
            }
            catch (Exception e)
            {
                Function.WriteColorText($"LOG:{e.Message}", ConsoleColor.Red);
                Function.DeleteClient(client);
            }
        }
    }
}