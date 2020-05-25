using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        private void SCORE(TcpClient client) //Отправка всех очков клиенту
        {
            try
            {
                for (long i = 0; i <= 100; i++)
                {
                    var info = Database.GetClientInfo(i);
                    if (info.Nick != "") Function.SendClientMessage(client, $"%SCORE:{info.Nick}:{info.Point}");
                }
            }
            catch (Exception e)
            {
                Function.WriteColorText($"MSG:{e.Message}", ConsoleColor.Red);
                Function.DeleteClient(client);
            }
        }
    }
}