using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace WorkerServer
{
    public static class Data
    {
        public const int Port = 908; //Порт сервера
        public static List<ThreadClient> Clients = new List<ThreadClient>(); //Инфа о подключённых сокетах(и потоках)
        public static bool ServerStart = true; // Сервер запущен?
        public static ILogger<Worker> _logger;//Systemd - логи

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
            public ClientInfo(long id, string email, string password, string nick, string coef, string level,
                string checklevel)
            {
                ID = id;
                Email = email;
                Password = password;

                Nick = nick;
                Coef = coef;
                Level = level;
                CheckLevel = checklevel;
            }

            public long ID { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Nick { get; set; }
            public string Coef { get; set; }
            public string Level { get; set; }
            public string CheckLevel { get; set; }
        }
    }
}