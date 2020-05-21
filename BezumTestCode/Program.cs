using System;
using System.Reflection;

namespace BezumTestCode
{
    class Program
    {
        static void Main()
        {             
            
            string message = "%LOG:ql@yandex.ru:glrb4gfgru"; //Любая входящая команда

            char ch = ':'; //Разделяющий символ
            string command = message.Substring(1, (message.IndexOf(ch) - 1)); //Команда 
            string[] arguments = message.Substring(message.IndexOf(ch) + 1).Split(new char[] { ch }); //Массив аргументов    

            try{ //Обращение к командам
                #region ConsoleWriteLine 
                Console.WriteLine("\n" + "Команда: " + command + " \n ");
                Console.WriteLine("Аргументы:\n----------");
                foreach (string s in arguments)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine("----------");
                #endregion

                Commands ComandClass = new Commands();
                ComandClass.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(ComandClass, new object[]{arguments});
            }
            catch(Exception ex)
            {
                Console.WriteLine("\nОшибка при поиске команды:\n----------\n" + ex + "\n----------");
            }

        }
    }
}
