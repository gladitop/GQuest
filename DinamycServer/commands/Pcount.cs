namespace DinamycServer
{
    public partial class Commands
    {
        public void PCOUNT(long clientId, int point) //Обновление очков говна
        {
            Database.UpdatePoint(new Data.InfoScoreAdd(clientId, point));//Да, вот так просто)
        }
    }
}