using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace DinamycServer
{
    public static class Data
    {
        public const int Port = 908;
        public static List<ClientInfo> ClientsInfo = new List<ClientInfo>(); //Инфа о клиентах
        public static List<ClientInfo> AddInfo = new List<ClientInfo>(); //Инфа о НОВЫХ клиентах
        
        public class ClientInfo //Инфо о клиенте (онлайн)
        {
            public ClientInfo(TcpClient socket, string email, string password, string nick, long id, long? point) //Инфо о клиенте
            {
                if(socket != null) {Socket = socket;}                      
                Email = email;
                Password = password;
                Nick = nick;
                Point = point;
                ID = id;
            }

            public ClientInfo() { }

            public TcpClient Socket { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Nick { get; set; }
            public long ID { get; set; }
            public long? Point { get; set; }
        }

    }
}