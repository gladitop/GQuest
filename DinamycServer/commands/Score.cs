using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        private void SCORE(TcpClient client, string[] atgument) //Отправка всех очков клиенту
        {
            //%SCORE:name:points
            for(int i = 0; i<=5; i++)
            {
             string[] gg = Database.GetScore(i);   
                Function.SendClientMessage(client, $"%SCORE:{gg[0]}:{gg[1]}"); 
                Console.WriteLine($"%SCORE:{gg[0]}:{gg[1]}");
            } 
        }
    }
}