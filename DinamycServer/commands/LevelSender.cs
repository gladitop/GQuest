using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {

        public static string[] argULVL = {"id"} ; // строка-подсказка с необходимыми аргументами
        private void ULVL(TcpClient client, string[] argumets)
        {       
            long id = long.Parse(argumets[0]);

            var info = Database.GetClientInfo(id);         
            Console.WriteLine($"\nfaa ID: {info.ID} |Lvl: {info.Level} |CompleteLevel: {info.LevelComplete}");

            var CompleteLevel = info.LevelComplete; //важно     ВСЕ ТЕСЛЫ ПРОЙДЕННЫЕ КЛИНТОМ
            string[] tableinfolevel = Database.CheckTableLevel(long.Parse(info.Level)); //важно    ВСЕ ТЕСТЫ С УРОВНЯ

            foreach(string til in tableinfolevel)
            {

            }
        }
    }
}