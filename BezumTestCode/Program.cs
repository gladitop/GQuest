using System;
using System.Reflection;

namespace BezumTestCode
{
    class Program
    {
        static void Main()
        {
            /*TODO: пересобрать входную строку. пример: 
            Входная строка: %REG:Gladi:Hello:123:points... (и др. данные)
            Выходные данные:
            .                commad = REG;
            .                argument[0] = Gladi
            .                argument[1] = Hello
            .                argument[2] = 123
            .                argument[3] = points
            .                argument[4] = Gladi                        
            */                       
            string command = "";                            //команда
            string[] argument = {"hi!", "hello!", "front"}; //аргументы
            try{
                Commands ComandClass = new Commands();
                ComandClass.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(ComandClass, new object[]{argument});
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ошибка при поиске команды:\n " + ex);
            }

        }
    }
}
