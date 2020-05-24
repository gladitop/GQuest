using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace DinamycServer
{
    public static class Data
    {
        public const int Port = 908;
        public static List<ClientInfo> ClientsInfo = new List<ClientInfo>(); //Инфа о клиентах

        #region public классы и перечисление

        //TODO Создать 1 класс, который будет иметь всю информацию о пользователе ClientInfo
        public class InfoScore //TODO удалить
        {
            public InfoScore(long id, long point) //Для добавление
            {
                UserID = id;
                Point = point;
            }

            public InfoScore(string email, long point) //Для показа
            {
                Email = email;
                Point = point;
            }

            public long UserID { get; set; } //Id пользователя
            public long Point { get; set; }

            public string Email { get; set; }
        }

        public class ClientInfo //Инфо о клиенте (онлайн)
        {
            public ClientInfo(TcpClient socket, string email, string password, string nick) //Онлайн
            {
                Socket = socket;
                Email = email;
                Password = password;
                Nick = nick;
                Point = 0;
            }

            public ClientInfo(string email, string password, string nick, long id, long point) //Инфо о клиенте
            {
                Email = email;
                Password = password;
                Nick = nick;
                Point = point;
                ID = id;
            }

            public static TcpClient Socket { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Nick { get; set; }
            public long ID { get; set; }

            public IPEndPoint IP => (IPEndPoint) Socket.Client.LocalEndPoint; //Я ОЧЕНЬ ленивый

            public long Point { get; set; }
        }

        #endregion
    }
}