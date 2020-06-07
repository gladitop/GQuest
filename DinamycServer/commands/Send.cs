using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        private void STEST(TcpClient client, string[] arguments)
        {
            try
            {
                for(int i = 1; i <= 1; i++)
                {
                    Console.WriteLine(i);
                    var info = Database.GetTest(i);
                    Console.WriteLine("2"); 
                    Function.SendClientMessage(client, info);    
                        Console.WriteLine("3");                    
                }
            }
            catch
            {
                Function.CheckEmptyClients(client);
                Console.WriteLine("2");
            }
        }
    }
}