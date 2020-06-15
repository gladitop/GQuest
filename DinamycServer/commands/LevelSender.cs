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

            string[] CompleteLevel = info.LevelComplete.Substring(info.LevelComplete.IndexOf(':') + 1).Split(new[] { ':' });//ВСЕ ТЕСТЫ ПРОЙДЕННЫЕ КЛИЕНТОМ

            string[] tableinfolevel = Database.CheckTableLevel(long.Parse(info.Level)); //ВСЕ ТЕСТЫ С УРОВНЯ

            foreach(string cmpl in CompleteLevel)
            {
                Console.WriteLine(cmpl);
            }
            foreach(string til in tableinfolevel)
            {
                Console.WriteLine(til);
            }
        }
    }
}