using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        private void SCORE(TcpClient client) //Отправка всех очков клиенту
        {
            for (long i = 0; i <= 100; i++)
            {
                var info = Database.GetClientInfo(i);
                if (info.Nick != "") Function.SendClientMessage(client, $"%SCORE:{info.Nick}:{info.Point}");
            }
        }
    }
}