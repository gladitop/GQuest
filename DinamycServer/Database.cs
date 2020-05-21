using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DinamycServer
{
    static public class Database
    {
        static public MySqlConnection connection { get; set; }

        static Database()//Подключение к базе данных
        {
            var ihost = "37.29.78.130";
            var iport = 3311;
            var idatabase = "test";
            var iusername = "admin";
            var ipassword = "030292";

            var connString = "Server=" + ihost + ";Database=" + idatabase + ";port=" + iport + ";User=" + iusername +
                             ";password=" + ipassword;

            connection = new MySqlConnection(connString);
            connection.Open();
        }

        static public void AddAccount(string email, string password, string nick) //Добавить аккаунт
        {
            var command = new MySqlCommand(
                $"INSERT INTO `accounts` (`w_email`, `w_password`, `w_nick`, `w_point`) VALUES ('{email}', '{password}', '{nick}', 0);",
                connection);
            Console.WriteLine($"В БД добавился новый клиент: {email}, {password}, {nick}");
            command.ExecuteNonQuery();
        }

        static public bool CheckPassword(string email, string password) //Проверка пароля в аккаунтах
        {
            Console.WriteLine($"Начало проверки пароля: {email}");
            var command = new MySqlCommand(
                $"SELECT w_password FROM accounts WHERE w_email = '{email}';",
                connection);
            var Ppassword = "";

            var reader = command.ExecuteReader();
            while (reader.Read()) Ppassword = reader.GetString("w_password");
            reader.Close();
            
            if (password == Ppassword) return true;
            return false;
        }

        /* Проверка данных для входа в аккаунт
           true - правильные данные
           false - НЕправильные данные  */
        static public bool LogAccount(string email, string password)
        {
            Console.WriteLine($"Вход в аккаунт: {email}:{password}");

            if (CheckEmail(email))
            {
                if (CheckPassword(password, email))
                {
                    //Вход усМешный!

                    return true;
                }
                else
                {
                    goto errorLint;
                }
            }
            else
            {
                goto errorLint;
            }
            
            errorLint:
            return false;
        }

        static public bool CheckEmail(string email) //Проверка почты в аккаунтах
        {
            Console.WriteLine($"Начало проверки почты: {email}");
            var command = new MySqlCommand(
                $"SELECT COUNT(*) FROM accounts WHERE w_email = '{email}';",
                connection);

            var count = (long) command.ExecuteScalar();

            if (count == 0)
                return false;
            return true;
        }
    }
}