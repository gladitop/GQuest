namespace DinamycServer
{
    public partial class Commands
    {
        private void PCOUNT(long clientId, int point) //Обновление очков говна
        {
            Database.UpdatePoint(new Data.InfoScoreAdd(clientId, point)); //Да, вот так просто)
        }
    }
}