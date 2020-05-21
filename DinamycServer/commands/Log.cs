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
        void LOG(object[] cmd) // %LOG:email:pass
        {         
            try{
                
                string email = ((string)cmd[0]);
                string pass = ((string)cmd[1]);
                Console.WriteLine($"Проверка логина:\nДанные: {email}, {pass}"); 
            }
            catch(Exception ex){Console.WriteLine("\nОшибка проверке аргументов:\n----------\n" + ex + "\n----------");}
            
            //проверка данных в БД
            //Send(%LOGOOD); или Send(%BLOG);
        }
    }
}