using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {

        public static string[] argULVL = {"id"} ; // строка-подсказка с необходимыми аргументами

        private void ULVL(TcpClient client, string[] argumets)
        {       
            long id = long.Parse(argumets[0]);//получаю id клиента

            var info = Database.GetClientInfo(id);//обращаюсь к бд, для получение остальной информации
            Console.WriteLine($"\nИнфа о клиенте ID: {info.ID} |Lvl: {info.Level} |CompleteLevel: {info.LevelComplete}");


            string[] tableinfolevel = Database.CheckTableLevel(long.Parse(info.Level)); //id всех тестов по профилям на уровне

            for(int i = 0; i < tableinfolevel.Length; i++) //id направления
            {
                if(tableinfolevel[i] != "0")
                {
                    string[] test = tableinfolevel[i].Split(new[] { '|' });

                    for(int p = 0; p < test.Length; p++) //id теста
                    {                        
                        string[] infotest = Database.CheckTableTest(p + 1);

                        Function.SendClientMessage(client, $"%TEST:{infotest[0]}:{infotest[1]}:{infotest[2]}:{infotest[3]}"); //отправка теста

                        string[] question_id = infotest[3].Split(new[] { '|' });
                        for(int l = 0; l < question_id.Length; l++) //id вопроса
                        {          
                            string[] infoquestion = Database.CheckTableQuestion(long.Parse(question_id[l])); //вопрос
                            string iq = "";
                            foreach(string st in infoquestion)
                            {
                                iq += $"{st}:";
                            }
                            Function.SendClientMessage(client, $"%QUEST:{i+1}:{infotest[0]}:{l}:{iq}"); //отправка вопроса
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Направление: {i+1} пусто");
                }                                                         
            }
        }
    }
}