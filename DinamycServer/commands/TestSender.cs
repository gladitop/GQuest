using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {

        public static string[] argTEST = {"Level_id"} ; // строка-подсказка с необходимыми аргументами
        private void TEST(TcpClient client, string[] argumets)
        {       
            Console.WriteLine(argumets[0]);
            string[] id_tests = Database.CheckTableLevel(long.Parse(argumets[0]));

            foreach(string i_t in id_tests)
            {
                if(i_t == "0") continue;

                string[] str = i_t.Split(new[] { '|' }); //Массив аргументов
                for(int i = 0; i < str.Length; i++)
                {
                    Function.WriteColorText("Test: " + str[i]);
                }

                Function.WriteColorText("Next: ");
            }
        }
    }
}