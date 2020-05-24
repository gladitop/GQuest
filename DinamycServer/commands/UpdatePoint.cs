using System;
namespace DinamycServer
{
    public partial class Commands
    {
        private void UPOINT(long clientId, int point) //Обновление очков говна
        {
            Database.UpdatePoint(new Data.InfoScore(clientId, point)); //Да, вот так просто)
        }
        
    }
}