using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdminGQuest.Other
{
    static public class Function//Функции (короткие команды)
    {
        public static void SendServer(string text)//Отправить в сервер
        {
            Data.Client.Client.Send(Encoding.UTF8.GetBytes(text));
        }

        public static string ReceiveServer()//Получить от сервера
        {
            byte[] buffer = new byte[1024];
            int i = Data.Client.Client.Receive(buffer);
            MessageBox.Show(Encoding.UTF8.GetString(buffer));
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
