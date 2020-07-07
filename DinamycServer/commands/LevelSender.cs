using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DinamycServer
{
    public partial class Commands
    {

        public void ULVL(object client, string[] argumets)
        {
            List<string> Sendinfo = new List<string>();

            var id = long.Parse(argumets[0]); //получаю id клиента
            var info = Database.GetClientInfo(id); //обращаюсь к бд, для получение остальной информации

            var tableinfolevel = Database.CheckTableLevel(long.Parse(info.LEVEL)); //id всех тестов по профилям на уровне

            for (var i = 0; i < tableinfolevel.Length; i++)
            {
                if (tableinfolevel[i] != "0")
                {
                    var test = tableinfolevel[i].Split(new[] { '|' }); //id теста

                    for (var p = 0; p < test.Length; p++)
                    {
                        var infotest = Database.CheckTableTest(long.Parse(test[0]));

                        Sendinfo.Add($"%TEST:{i + 1}:{infotest[0]}:{infotest[1]}:{infotest[2]}:{infotest[3]}");

                        var question_id = infotest[3].Split(new[] { '|' });
                        for (var l = 0; l < question_id.Length; l++) //id вопроса
                        {
                            var infoquestion = Database.CheckTableQuestion(long.Parse(question_id[l])); //вопрос

                            var iq = "";
                            foreach (var st in infoquestion) iq += $"{st}:";
                            Sendinfo.Add($"%QUEST:{i + 1}:{infotest[0]}:{question_id[l]}:{iq}");
                        }
                    }
                }
            }
            Function.SendClientMessage(client, Sendinfo);
        }
    }
}