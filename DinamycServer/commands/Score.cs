using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        public void SCORE(TcpClient client, string name, long point) //Отправка всех очков клиенту
        {
            var infos = Database.GetScore();

            //%SCORE:name:points
            foreach (var info in infos) Function.SendClientMessage(client, $"%SCORE:{info.Email}:{info.Point}");
        }
    }
}