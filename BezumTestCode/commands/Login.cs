using System;
namespace BezumTestCode
{
    public partial class Commands
    {
        void LOG(object[] dd) // %LOG:email:pass
        {          
            string email = ((string)dd[0]);
            string pass = ((string)dd[1]);
            Console.WriteLine($"Логин пользователя:\nДанные: {email}, {pass}");
            //проверка данных в БД
            //Send(%LOGOOD); или Send(%BLOG);
        }
    }
}