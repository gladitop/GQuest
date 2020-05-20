using System;
namespace BezumTestCode
{
    public partial class Commands
    {
        void REG(object[] dd) // %REG:email:pass:clientName
        {          
            string email = ((string)dd[0]);
            string pass = ((string)dd[1]);
            string nick = ((string)dd[2]);
            Console.WriteLine($"Регистрация пользователя:\nДанные: {email}, {pass}, {nick}");
            //проверка данных в БД
            //Send(%REGOD); или Send(%BREG);
        }   
    }
}