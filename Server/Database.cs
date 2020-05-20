using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Server
{
    public class Database
    {
        public Database()
        {
            var ihost = "37.29.78.130";
            var iport = 3311;
            var idatabase = "test";
            var iusername = "admin";
            var ipassword = "030292";

            var connString = "Server=" + ihost + ";Database=" + idatabase
                             + ";port=" + iport + ";User=" + iusername + ";password=" + ipassword;

            //Server=37.29.78.130;Database=olhdata;port=030292;User Id=admin;password=030292
            connection = new MySqlConnection(connString);
            connection.Open();
        }

        public MySqlConnection connection { get; set; }

        public List<Data.InfoScore> GetScore()
        {
            var command = new MySqlCommand(
                "SELECT w_email, w_point FROM accounts WHERE w_point != 'NULL';",
                connection);

            var info = new List<Data.InfoScore>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var email = reader.GetString("w_email");
                var point = reader.GetInt64("w_point");

                info.Add(new Data.InfoScore(email, point));
            }

            return info;
        }

        public void AddPoint(Data.InfoPoint infoPoint) //Обновить 
        {
            var command = new MySqlCommand(
                $"UPDATE accounts SET w_point = '{infoPoint.Point}' WHERE w_id = '{infoPoint.UserID}';",
                connection);
            command.ExecuteNonQuery();
            Console.WriteLine($"Добавление в бд очки: id= {infoPoint.UserID}, points= {infoPoint.Point}");
        }

        public Data.ClientInfoOffile GetClientInfo(string email) //Получение инфо о клиенте
        {
            var command = new MySqlCommand(
                $"SELECT * FROM accounts WHERE w_email = '{email}';",
                connection);
            long id = 0;
            var emailInfo = "";
            var password = "";
            var nick = "";
            long? point = null;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt64("w_id");
                emailInfo = reader.GetString("w_email");
                password = reader.GetString("w_password");
                nick = reader.GetString("w_nick");
                point = reader.GetInt64("w_point"); //NULL!!!
            }

            reader.Close();
            return new Data.ClientInfoOffile(id, email, password, nick, point);
        }

        public bool CheckPassword(string Pemail, string Ppassword) //Проверка пароля в аккаунтах
        {
            Console.WriteLine($"Начало проверки пароля: {Pemail}");
            var command = new MySqlCommand(
                $"SELECT * FROM accounts WHERE w_email = '{Pemail}';",
                connection);
            var password = "";

            var reader = command.ExecuteReader();
            while (reader.Read()) password = reader.GetString("w_password");
            reader.Close();

            Console.WriteLine(Ppassword + " " + password);
            if (Ppassword == password) return true;
            return false;
        }

        public bool CheckEmail(string email) //Проверка почты в аккаунтах
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

        public void AddAccount(string email, string password, string nick) //Добавить аккаунт
        {
            var command = new MySqlCommand(
                $"INSERT INTO `accounts` (`w_email`, `w_password`, `w_nick`, `w_point`) VALUES ('{email}', '{password}', '{nick}', 0);",
                connection);
            Console.WriteLine($"В БД добавился новый клиент: {email}, {password}, {nick}");
            command.ExecuteNonQuery();
        }
    }
}