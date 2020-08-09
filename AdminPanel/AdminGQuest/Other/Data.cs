using System.Net.Sockets;
using System.Text;

namespace AdminGQuest.Other
{
    public static class Data
    {
        #region Параметры

        public const string PathSave = "Settings.json";//Вот тут будет сохранятся настройки
        public const string IPServer = "192.168.0.15";//Ip сервера
        public const int PortServer = 908;//Порт сервера

        #endregion

        #region Данные

        public static SettingsData Settings { get; set; }
        public static TcpClient Client { get; set; }

        #endregion
    }
}
