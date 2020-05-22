using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace DinamycServer
{
    public static class Database
    {
        static Database() //Подключение к базе данных
        {
            var ihost = "37.29.78.130";
            var iport = 3311;
            var idatabase = "test";
            var iusername = "admin";
            var ipassword = "030292";

            var connString = "Server=" + ihost + ";Database=" + idatabase + ";port=" + iport + ";User=" + iusername + ";password=" + ipassword;

            connection = new MySqlConnection(connString);
            connection.Open();
        }

        public static MySqlConnection connection { get; set; }

        static  public List<Data.InfoScoreShow> GetScore()//Получить все очки говна
        {
            var command = new MySqlCommand(
                "SELECT w_email, w_point FROM accounts WHERE w_point != 'NULL';",
                connection);

            var info = new List<Data.InfoScoreShow>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var email = reader.GetString("w_email");
                var point = reader.GetInt64("w_point");

                info.Add(new Data.InfoScoreShow(email, point));
            }

            return info;
        }

        static public void UpdatePoint(Data.InfoScoreAdd infoPoint) //Обновить очки говна
        {
            var command = new MySqlCommand(
                $"UPDATE accounts SET w_point = '{infoPoint.Point}' WHERE w_id = '{infoPoint.UserID}';",
                connection);
            command.ExecuteNonQuery();
            Console.WriteLine($"Добавление в бд очки: id= {infoPoint.UserID}, points= {infoPoint.Point}");
        }
        
        public static void AddAccount(string email, string password, string nick) //Добавить аккаунт
        {
            var command = new MySqlCommand(
                $"INSERT INTO `accounts` (`w_email`, `w_password`, `w_nick`, `w_point`) VALUES ('{email}', '{password}', '{nick}', 0);",
                connection);
            Console.WriteLine($"В БД добавился новый клиент: {email}, {password}, {nick}");
            command.ExecuteNonQuery();
        }
        public static bool CheckEmail(string email) //Проверка почты в аккаунтах
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
        public static bool CheckPassword(string email, string password) //Проверка пароля в аккаунтах
        {
            Console.WriteLine($"Начало проверки пароля: {email}");
            var command = new MySqlCommand(
                $"SELECT w_password FROM accounts WHERE w_email = '{email}';",
                connection);
            var Ppassword = "";

            var reader = command.ExecuteReader();
            while (reader.Read()) Ppassword = reader.GetString("w_password");
            reader.Close();
            Console.WriteLine(Ppassword);

            if (password == Ppassword) return true;
            else return false;
        }
    }
}