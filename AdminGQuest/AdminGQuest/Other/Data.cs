using System.Net.Sockets;
using System.Text;

namespace AdminGQuest.Other
{
    public static class Data
    {
        #region Параметры

        public const string PathSave = "Settings.json";//Вот тут будет сохранятся настройки
        public const string IPServer = "";//Ip сервера
        public const int PortServer = 908;//Порт сервера

        #endregion

        #region Данные

        public static SettingsData Settings { get; set; }
        public static TcpClient Client { get; set; }

        #endregion

        #region Короткие команды

        public static void SendServer(string text)//Отправить в сервер
        {
            Data.Client.Client.Send(Encoding.UTF8.GetBytes(text));
        }

        public static string ReceiveServer()//Получить от сервера
        {
            byte[] buffer = new byte[1024];
            int i = Data.Client.Client.Receive(buffer);
            return Encoding.UTF8.GetString(buffer);
        }

        #endregion
    }
}
