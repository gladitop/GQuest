using System;

namespace DinamycServer
{
    public partial class Commands
    {
        private void REG(object[] cmd) // %REG:email:pass:clientName
        {
            try
            {
                var email = (string) cmd[0];
                var pass = (string) cmd[1];
                var nick = (string) cmd[2];
                Console.WriteLine($"Проверка регистра:\nДанные: {email}, {pass}, {nick}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nОшибка проверке аргументов:\n----------\n" + ex + "\n----------");
            }

            //проверка данных в БД
            //Send(%REGOD); или Send(%BREG);
        }
    }
}