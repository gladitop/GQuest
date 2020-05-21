using System;
namespace DinamycServer
{
    public partial class Commands
    {
        void REG(object[] cmd) // %REG:email:pass:clientName
        {          
            try{
               string email = ((string)cmd[0]);
                string pass = ((string)cmd[1]);
                string nick = ((string)cmd[2]);
                Console.WriteLine($"Проверка регистра:\nДанные: {email}, {pass}, {nick}");
            }
            catch(Exception ex){Console.WriteLine("\nОшибка проверке аргументов:\n----------\n" + ex + "\n----------");}
            //проверка данных в БД
            //Send(%REGOD); или Send(%BREG);
        }   
    }
}