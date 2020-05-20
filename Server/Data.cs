using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Org.BouncyCastle.Crypto.Tls;

namespace Server
{
    static public class Data
    {
        #region Параметры

        public const int Port = 908;

        #endregion

        #region Сама инфа 

        static public List<ClientInfoOnly> ClientsInfo = new List<ClientInfoOnly>();//Инфа о клиентах

        #endregion

        #region Разное

        public class InfoScore
        {
            public string Email { get; set; }
            public long Point { get; set; }

            public InfoScore(string email, long point)
            {
                Email = email;
                Point = point;
            }
        }
        
        public class InfoPoint//Инфо о очках
        {
            public long UserID { get; set; }//Id пользователя
            public long? Point { get; set; }

            public InfoPoint(long id, long? point = null)
            {
                UserID = id;
                Point = point;
            }
        }

        public class ClientInfoOffile//Инфо о клиенте (офнлайн)
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Nick { get; set; }
            public long? Point { get; set; }//Очки (Может быть значение NULL)
            public long ID { get; set; }

            public ClientInfoOffile(long id, string email, string password, string nick, long? point = null)
            {
                ID = id;
                Email = email;
                Password = password;
                Nick = nick;
                Point = null;
            }
        }

        public class ClientInfoOnly//Инфо о клиенте (онлайн)
        {
            public TcpClient Socket { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Nick { get; set; }
            public IPEndPoint IP
            {
                get => (IPEndPoint)Socket.Client.LocalEndPoint;//Я ОЧЕНЬ ленивый
            }
            public long? Point { get; set; }//Очки (Может быть значение NULL)

            public ClientInfoOnly(TcpClient socket, string email, string password, string nick)
            {
                Socket = socket;
                Email = email;
                Password = password;
                Nick = nick;
                Point = null;
            }
        }

        #endregion
    }
}