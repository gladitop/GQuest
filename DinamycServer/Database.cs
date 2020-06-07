using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

            var connString = "Server=" + ihost + ";Database=" + idatabase + ";port=" + iport + ";User=" + iusername + ";password=" + ipassword;

            try //Проверка на поключение к бд
            {
                connection = new MySqlConnection(connString);
                connection.Open();
            }
            catch (Exception ex)
            {
                Function.WriteColorText("Не удалось подключиться к бд: \n" + ex, ConsoleColor.DarkYellow);
            }

        }

        public static MySqlConnection connection { get; set; }

        #region GetInfo

        public static Data.QuestionsInfo GetQuestionInfo(long id)
        {
            var command = new MySqlCommand($"SELECT * FROM `questions` WHERE tests_id = '{id}';", connection);

            var questionsInfo = new Data.QuestionsInfo();
            List<Data.QuestionInfo> questionInfo = new List<Data.QuestionInfo>();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                for (int i = 0; i == 5; i++)
                {
                    //Получение
                    string title = "";//TODO
                    string q1 = reader.GetString("text");//!!!
                    string q2 = reader.GetString("text");
                    string q3 = reader.GetString("text");
                    string q4 = reader.GetString("text");
                    string q5 = reader.GetString("text");
                    string q6 = reader.GetString("text");

                    //Инцилизация

                    questionInfo.Add(new Data.QuestionInfo(title, q1, q2, q3, q4, q5, q6));
                }
            }

            questionsInfo = new Data.QuestionsInfo(questionInfo[1], questionInfo[2], questionInfo[3], questionInfo[4], questionInfo[5]);
            return questionsInfo;
        }

        public static Data.ClientInfo GetClientInfo(string email) //Получение инфо о клиенте (по email)
        {
            var command = new MySqlCommand($"SELECT * FROM `accounts` WHERE w_email = '{email}';", connection);

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
                try { point = reader.GetInt64("w_point"); }
                catch { Console.WriteLine("point = null"); }
            }
            reader.Close();
            command.Dispose();

            return new Data.ClientInfo(null, email, password, nick, id, point);
        }
        public static Data.ClientInfo GetClientInfo(long id) //Получение инфо о клиенте (по id)
        {
            var command = new MySqlCommand($"SELECT * FROM `accounts` WHERE w_id = {id};", connection);

            var email = "";
            var password = "";
            var nick = "";
            long? point = null;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                email = reader.GetString("w_email");
                password = reader.GetString("w_password");
                nick = reader.GetString("w_nick");
                try { point = reader.GetInt64("w_point"); }
                catch { Console.WriteLine("point = null"); }
            }
            reader.Close();
            command.Dispose();

            return new Data.ClientInfo(null, email, password, nick, id, point);
        }

        #endregion

        #region Account managment

        public static void AddAccount(string email, string password, string nick) //Добавить аккаунт
        {
            var command = new MySqlCommand($"INSERT INTO `accounts` (`w_email`, `w_password`, `w_nick`) VALUES ('{email}', '{password}', '{nick}');", connection);
            command.ExecuteNonQuery();
            Console.WriteLine($"В БД добавился новый клиент: {email}, {password}, {nick}");
        }

        public static bool CheckEmail(string email) //Проверка почты в аккаунтах
        {
            var command = new MySqlCommand($"SELECT COUNT(*) FROM accounts WHERE w_email = '{email}';", connection);
            var count = (long)command.ExecuteScalar();

            if (count == 0) return false;
            else return true;
        }

        public static bool CheckPassword(string email, string password) //Проверка пароля в аккаунтах
        {
            var command = new MySqlCommand($"SELECT w_password FROM accounts WHERE w_email = '{email}';", connection);
            var Ppassword = "";
            var reader = command.ExecuteReader();

            while (reader.Read()) { Ppassword = reader.GetString("w_password"); }
            reader.Close();

            if (password == Ppassword) return true;
            else return false;
        }

        #endregion

        #region Interactive
        public static void UpdatePoint(long id, long point) //Обновить очки
        {
            Console.WriteLine(point);
            var command = new MySqlCommand($"UPDATE accounts SET w_point = '{point}' WHERE w_id = '{id}';", connection);
            command.ExecuteNonQuery();
            Console.WriteLine($"Добавление в бд очки: id= {id}, points= {point}");
        }

        #endregion
    }
}