using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DinamycServer
{
    public partial class Commands
    {
        public static string[] argULVL = { "id" }; // строка-подсказка с необходимыми аргументами

        public void ULVL(object client, string[] argumets)
        {
            var id = long.Parse(argumets[0]); //получаю id клиента

            var info = Database.GetClientInfo(id); //обращаюсь к бд, для получение остальной информации
            //Console.WriteLine($"\nИнфа о клиенте ID: {info.ID} |Lvl: {info.Level} |CompleteLevel: {info.LevelComplete}");


            var tableinfolevel =
                Database.CheckTableLevel(long.Parse(info.Level)); //id всех тестов по профилям на уровне

            for (var i = 0; i < tableinfolevel.Length; i++)
            {
                if (tableinfolevel[i] != "0")
                {
                    var test = tableinfolevel[i].Split(new[] { '|' }); //id теста

                    for (var p = 0; p < test.Length; p++)
                    {
                        var infotest = Database.CheckTableTest(long.Parse(test[0]));

                        Function.SendClientMessage(client,
                            $"%TEST:{i + 1}:{infotest[0]}:{infotest[1]}:{infotest[2]}:{infotest[3]}"); //отправка теста

                        var question_id = infotest[3].Split(new[] { '|' });
                        for (var l = 0; l < question_id.Length; l++) //id вопроса
                        {
                            var infoquestion = Database.CheckTableQuestion(long.Parse(question_id[l])); //вопрос

                            var iq = "";
                            foreach (var st in infoquestion) iq += $"{st}:";
                            Function.SendClientMessage(client,
                                $"%QUEST:{i + 1}:{infotest[0]}:{question_id[l]}:{iq}"); //отправка вопроса направление:тест:вопрос:сам вопрос
                            Task.Delay(10).Wait();
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Направление: {i + 1} пусто");
                }
            }
        }
    }
}