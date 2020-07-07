using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography;
using System.Text;
using System;
using Org.BouncyCastle.Asn1.Crmf;

namespace DinamycServer
{
    public static class Data
    {
        public const int Port = 908; //Порт сервера
        public static StreamWriter Logger;
        public static List<ThreadClient> Clients = new List<ThreadClient>(); //Инфа о подключённых сокетах(и потоках)

        public class ThreadClient
        {
            public ThreadClient(TcpClient tpCl, Thread TrCl)
            {
                TpClient = tpCl;
                ThrClient = TrCl;
            }

            public TcpClient TpClient { get; }
            public Thread ThrClient { get; }
        }

        public class ClientInfo //Инфа о клиенте
        {
            public ClientInfo(long id, string email, string password, string nick, string coef, string level, string checklevel)
            {
                ID = id;
                EMAIL = email;
                PASSWORD = password;

                NICK = nick;
                COEF = coef;
                LEVEL = level;
                CHECKLEVEL = checklevel;
            }

            public long ID { get; set; }
            public string EMAIL { get; set; }
            public string PASSWORD { get; set; }

            public string NICK { get; set; }
            public string COEF { get; set; }
            public string LEVEL { get; set; }
            public string CHECKLEVEL { get; set; }
        }
    }
}