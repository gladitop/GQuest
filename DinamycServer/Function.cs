using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DinamycServer
{
    static public class Function//Функции
    {
        static public void SendClientMessage(TcpClient client, string message)//Отравить клиенту сообщение
        {
            client.Client.Send(Encoding.UTF8.GetBytes(message));
        }
    }
}