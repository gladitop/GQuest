using System;
using MySql.Data.MySqlClient;

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

            var connString = "Server=" + ihost + ";Database=" + idatabase + ";port=" + iport + ";User=" + iusername +
                             ";password=" + ipassword;

            connection = new MySqlConnection(connString);
            connection.Open();
        }

        public static MySqlConnection connection { get; set; }

        #region GetInfo

        public static Data.ClientInfo GetClientInfo(string email) //Получение инфо о клиенте (по email)
        {
            var command = new MySqlCommand(
                $"SELECT * FROM `accounts` WHERE w_email = '{email}';",
                connection);

            long id = 0;
            var emailInfo = "";
            var password = "";
            var nick = "";
            long point = 0;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt64("w_id");
                emailInfo = reader.GetString("w_email");
                password = reader.GetString("w_password");
                nick = reader.GetString("w_nick");
                point = reader.GetInt64("w_point");
            }

            reader.Close();
            command.Dispose();

            //Console.WriteLine($"{id}, {emailInfo}, {password}, {nick}");
            return new Data.ClientInfo(email, password, nick, id, point);
        }

        public static Data.ClientInfo GetClientInfo(long id) //Получение инфо о клиенте (по id)
        {
            var command = new MySqlCommand(
                $"SELECT * FROM `accounts` WHERE w_id = {id};",
                connection);

            var email = "";
            var password = "";
            var nick = "";
            long point = 0;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                email = reader.GetString("w_email");
                password = reader.GetString("w_password");
                nick = reader.GetString("w_nick");
                point = reader.GetInt64("w_point");
            }

            reader.Close();
            command.Dispose();
            //string email, string password, string nick)
            return new Data.ClientInfo(email, password, nick, id, point);
        }

        #endregion

        #region Account managment

        public static void UpdatePoint(int point, long id) //Обновить очки
        {
            var command = new MySqlCommand(
                $"UPDATE accounts SET w_point = '{point}' WHERE w_id = '{id}';",
                connection);
            command.ExecuteNonQuery();
            Console.WriteLine($"Добавление в бд очки: id= {id}, points= {id}");
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

        #endregion
    }
}