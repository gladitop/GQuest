using System;
using System.Reflection;

namespace BezumTestCode
{
    class Program
    {
        static void Main()
        {
            string command = ""; //команда, которая приходит
            string[] argument = {"hi!", "hello!", "front"};

            Commands tom = new Commands();
            tom.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(tom, new object[]{argument});

        }
    }
}
