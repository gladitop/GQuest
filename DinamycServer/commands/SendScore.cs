using System;
using System.Net.Sockets;
using System.Collections.Generic;

namespace DinamycServer
{
    public partial class Commands
    {
        private void SCORE(TcpClient client) //Отправка всех очков клиенту
        {
            //%SCORE:name:points
            for(int i = 0; i<=5; i++)
            {               
                //var nick = Database.GetClientInfo(i).Nick;
                //var point = Database.GetClientInfo(i).Point;
                
               // Function.SendClientMessage(client, $"%SCORE:{nick}:{point}"); 
               // Console.WriteLine($"%SCORE:{nick}:{point}");
            } 
            Console.WriteLine("good");
        }
    }
}