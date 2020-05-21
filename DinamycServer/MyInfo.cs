/*using System;
namespace Server
{
	public partial class Commands   //уточнение "partial" - для связи класса
	{
    	void command_name(object[] dd)
    	{          
    	   // string argument = ((string)dd[0]);
    	}    
	}
}

using System;
using System.Reflection;
namespace Server
{
	class Program
    {
        static void Main()
        {                 
            string command = "";                            //команда
            string[] argument = {"hi!", "hello!", "front"}; //аргументы

            try{
            	Commands tom = new Commands(); //Поиск команды, и оптравка аргументов
            	tom.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(tom, new object[]{argument});
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ошибка при поиске команды:\n " + ex);
            }
        }
    }
}*/