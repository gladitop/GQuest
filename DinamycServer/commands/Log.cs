using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace DinamycServer
{
    public partial class Commands
    {
        public static Database database { get; set; }
        void LOG(TcpClient client, string[] argumets) // %LOG:email:pass
        {         
            try{
                
                string email = argumets[0];
                string pass = argumets[1];
            }
            catch(Exception ex){Console.WriteLine("\nОшибка при проверке аргументов:\n----------\n" + ex + "\n----------");}
                      
            //проверка данных в БД //TODO
            //Send(%LOGOOD); или Send(%BLOG); //TODO

            void Send(string msg)
            {
                client.Client.Send(Encoding.UTF8.GetBytes(msg));
            }
        }
    }
}