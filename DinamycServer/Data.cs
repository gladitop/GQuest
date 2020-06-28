using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DinamycServer
{
    public static class Data
    {
        public const int Port = 908; //Порт сервера
        public static StreamWriter Logger;
        public static List<ThreadClient> Clients = new List<ThreadClient>(); //Инфа о подключённых сокетах(и потоках)
        public static List<TcpClient> AdminSocket = new List<TcpClient>();// Сокеты подключеных админов (для проверки)
        
        public class ThreadClient
        {
            public ThreadClient(TcpClient tpCl, Thread TrCl)
            {
                TpClient = tpCl;
                ThrClient = TrCl;
            }
            public TcpClient TpClient{get;}
            public Thread ThrClient{get;}
        }

        public class ClientInfo //Инфа о клиенте
        {
            public ClientInfo(long id, string email, string password, string nick, string coef,string level)
            {
                ID = id;
                Email = email;
                Password = password;

                Nick = nick;
                Coef = coef;
                Level = level;
            }
            
            public long ID { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }        
              
            public string Nick { get; set; }
            public string Coef { get; set; }
            public string Level { get; set; }
        }
    }
}