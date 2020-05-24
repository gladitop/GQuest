using System;
using System.Net.Sockets;
namespace DinamycServer
{
    public partial class Commands
    {
        private void UPOINT(long clientId, int point) //Обновление очков говна
        {
           // Database.UpdatePoint(new Data.InfoScore(clientId, point));
        }

        private void UPD(TcpClient client, string[] id)
        {
            long tt = long.Parse(id[0]);
            var gg = Database.GetClientInfo(tt);
            //Function.SendClientMessage(client, $"{gg.Email} {gg.ID} {gg.Nick} {gg.Password} {gg.Point}");
        }
        
    }
}