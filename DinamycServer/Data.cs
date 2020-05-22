using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace DinamycServer
{
    public static class Data
    {
        #region Параметры

        public const int Port = 908;

        #endregion

        #region Сама инфа

        public static List<ClientInfoOnly> ClientsInfo = new List<ClientInfoOnly>(); //Инфа о клиентах

        #endregion

        #region Разное

        public class InfoScoreShow //Инфо о очках (для показа)
        {
            public InfoScoreShow(string email, long point)
            {
                Email = email;
                Point = point;
            }

            public string Email { get; set; }
            public long Point { get; set; }
        }

        public class InfoScoreAdd //Инфо о очках (для Добавление)
        {
            public InfoScoreAdd(long id, long? point = null)
            {
                UserID = id;
                Point = point;
            }

            public long UserID { get; set; } //Id пользователя
            public long? Point { get; set; }
        }

        public class ClientInfoOffile //Инфо о клиенте (офнлайн)
        {
            public ClientInfoOffile(long id, string email, string password, string nick, long? point = null)
            {
                ID = id;
                Email = email;
                Password = password;
                Nick = nick;
                Point = null;
            }

            public string Email { get; set; }
            public string Password { get; set; }
            public string Nick { get; set; }
            public long? Point { get; set; } //Очки (Может быть значение NULL)
            public long ID { get; set; }
        }

        public class ClientInfoOnly //Инфо о клиенте (онлайн)
        {
            public ClientInfoOnly(TcpClient socket, string email, string password, string nick)
            {
                Socket = socket;
                Email = email;
                Password = password;
                Nick = nick;
                Point = null;
            }

            public TcpClient Socket { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Nick { get; set; }

            public IPEndPoint IP => (IPEndPoint) Socket.Client.LocalEndPoint; //Я ОЧЕНЬ ленивый

            public long? Point { get; set; } //Очки (Может быть значение NULL)
        }

        #endregion
    }
}