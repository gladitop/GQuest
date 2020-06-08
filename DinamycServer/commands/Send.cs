using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        /*private void STEST(TcpClient client, string[] arguments)
        {
            
            try
            {
                for(int i = 1; i <= 1; i++)
                {
                    Console.WriteLine(i);
                    var info = Database.GetQuestionInfo(i);
                    Console.WriteLine("2");

                    for (int ii = 0; ii == 5; ii++)
                    {
                        string send = $"STEST%:{info.Question1.Answer1}:{info.Question1.Answer2}:{info.Question1.Answer3}:{info.Question1.Answer4}:{info.Question1.Answer5}:" +
                            $"{info.Question1.Answer6}:";
                        Function.SendClientMessage(client, send);
                    }

                    Console.WriteLine("3");                    
                }
            }
            catch
            {
                Function.CheckEmptyClients(client);
                Console.WriteLine("2");
            }
        }*/
    }
}