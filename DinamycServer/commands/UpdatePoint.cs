using System;
using System.Net.Sockets;
namespace DinamycServer
{
    public partial class Commands
    {
        private void UPOINT(long clientId, int point) //TODO доделать метод с изменением очков
        {
           // Database.UpdatePoint(new Data.InfoScore(clientId, point));
        }
    }
}