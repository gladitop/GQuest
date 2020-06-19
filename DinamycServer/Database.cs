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
                Function.WriteColorText("Error to connected BD: \n" + ex, ConsoleColor.DarkYellow);
            }
        }

        public static MySqlConnection connection { get; set; }

        #region GetClientInfo      
        public static Data.ClientInfo GetClientInfo(string email) //Получение инфо о клиенте (по email)
        {
            var command = new MySqlCommand($"SELECT * FROM `accounts` WHERE w_email = '{email}';", connection);

            long id = 0;
            var emailInfo = "";
            var password = "";
            var nick = "";
            var coef = "";
            var level = "";
            var levelcomplete = "";

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt64("w_id");
                emailInfo = reader.GetString("w_email");
                password = reader.GetString("w_password");
                nick = reader.GetString("w_nick");
                coef = reader.GetString("coef");
                level = reader.GetString("w_level");
                levelcomplete = reader.GetString("w_chek_level");
            }
            reader.Close();
            command.Dispose();

            return new Data.ClientInfo(null, id, email, password, nick,coef,level, levelcomplete);
        }
        public static Data.ClientInfo GetClientInfo(long id) //Получение инфо о клиенте (по id)
        {
            var command = new MySqlCommand($"SELECT * FROM `accounts` WHERE w_id = {id};", connection);

            var email = "";
            var password = "";
            var nick = "";
            var coef = "";
            var level = "";
            var levelcomplete = "";
            var isAdmin = false;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                email = reader.GetString("w_email");
                password = reader.GetString("w_password");
                nick = reader.GetString("w_nick");
                coef = reader.GetString("coef");
                level = reader.GetString("w_level");
                levelcomplete = reader.GetString("w_chek_level");
                isAdmin = reader.GetBoolean("IsAdmin");
            }
            reader.Close();
            command.Dispose();

            var info = new Data.ClientInfo(null, id, email, password, nick, coef, level, levelcomplete);
            info.IsAdmin = true;
            return info;
        }

        #endregion

        #region Account managment

        public static void AddAccount(string email, string password, string nick) //Добавить аккаунт (просто добавление)
        {
            var command = new MySqlCommand($"INSERT INTO `accounts` (`w_email`, `w_password`, `w_nick`) VALUES ('{email}', '{password}', '{nick}');", connection);
            command.ExecuteNonQuery();
            Function.WriteColorText($"Add new client in BD: {email}, {password}, {nick}");
        }
        
        public static void AddAccount(string email, string password, string nick, string coef, string level, string levelcomplete,
            bool isAdmin) //Добавить аккаунт (Для админов)
        {
            var command = new MySqlCommand($"INSERT INTO `accounts` (`w_email`, `w_password`, `w_nick`) VALUES ('{email}', '{password}', '{nick}');", connection);//TODO:Дописать!
            command.ExecuteNonQuery();
            Function.WriteColorText($"Add new client in BD: {email}, {password}, {nick}, {coef}, {level}, {levelcomplete}, {isAdmin}");
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
        public static void UpdateCoefficients(long id, string coef) //Обновить очки
        {
            var command = new MySqlCommand($"UPDATE accounts SET coef = '{coef}' WHERE w_id = '{id}';", connection);
            command.ExecuteNonQuery();
            Function.WriteColorText($"Set coeficents: id= {id}, coeficent = {coef}");
        }
        public static void UpdateLevel(long id, string lvl)
        {
            var command = new MySqlCommand($"UPDATE accounts SET w_level = '{lvl}' WHERE w_id = '{id}';", connection);
            command.ExecuteNonQuery();
            Function.WriteColorText($"Set level: id= {id}, level= {lvl}");
        }

        public static string[] CheckTableLevel(long id)
        {
            var command = new MySqlCommand($"SELECT * FROM `level` WHERE w_id = {id};", connection);

            string[] info = new string[6];

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                info[0] = reader.GetString("pf_IT");
                info[1] = reader.GetString("pf_ROBO");
                info[2] = reader.GetString("pf_HT");
                info[3] = reader.GetString("pf_PROM");
                info[4] = reader.GetString("pf_NANO");
                info[5] = reader.GetString("pf_BIO");
            }
            reader.Close();
            command.Dispose();

            return info;
        }
        public static string[] CheckTableTest(long id)
        {
            var command = new MySqlCommand($"SELECT * FROM `tests` WHERE w_id = {id};", connection);

            string[] info = new string[4];
            info[0] = Convert.ToString(id);

            var reader = command.ExecuteReader();
            while (reader.Read())
            {      
                info[1] = reader.GetString("name");
                info[2] = reader.GetString("text");
                info[3] = reader.GetString("questions");
            }
            reader.Close();
            command.Dispose();

            return info;
        }       
        public static string[] CheckTableQuestion(long id)
        {
            var command = new MySqlCommand($"SELECT * FROM `quest` WHERE w_id = {id};", connection);

            string[] info = new string[8];

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                info[0] = reader.GetString("text"); 
                info[1] = reader.GetString("f1"); 
                info[2] = reader.GetString("f2"); 
                info[3] = reader.GetString("f3"); 
                info[4] = reader.GetString("f4"); 
                info[5] = reader.GetString("f5"); 
                info[6] = reader.GetString("f6"); 
                info[7] = reader.GetString("mark"); 

            }
            reader.Close();
            command.Dispose();

            return info;
        }
        
        #endregion
    }
}