using MySql;
using MySql.Data.MySqlClient;
using System;

namespace Server
{
    public class Database
    {
        public MySqlConnection connection { get; set; }

        public Database()
        {
            string ihost = "37.29.78.130";
            int iport = 3311;
            string idatabase = "test";
            string iusername = "admin";
            string ipassword = "030292";

            string connString = "Server=" + ihost + ";Database=" + idatabase
                                + ";port=" + iport + ";User=" + iusername + ";password=" + ipassword;

            //Server=37.29.78.130;Database=olhdata;port=030292;User Id=admin;password=030292
            connection = new MySqlConnection(connString);
            connection.Open();
        }

        public void AddPoint(Data.InfoPoint infoPoint)
        {
            MySqlCommand command = new MySqlCommand(
                $"UPDATE `accounts` SET 'point' = 'point' + {infoPoint.Point} WHERE id = {infoPoint.UserID};",
                connection);
            command.ExecuteNonQuery();
        }

        public Data.ClientInfoOffile GetClientInfo(string email) //Получение инфо о клиенте
        {
            MySqlCommand command = new MySqlCommand(
                $"SELECT * FROM `accounts` WHERE email = '{email}';",
                connection);

            long id = 0;
            string emailInfo = "";
            string password = "";
            string nick = "";
            long? point = null;
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt64("id");
                emailInfo = reader.GetString("email");
                password = reader.GetString("password");
                nick = reader.GetString("nick");
                Console.WriteLine(nick);
                point = reader.GetInt64("point");//NULL!!!
            }
            
            Console.WriteLine($"{id}, {emailInfo}, {password}, {nick}");
            return new Data.ClientInfoOffile(id, email, password, nick, point);
        }

        public bool CheckPassword(string password, string email) //Проверка пароля в аккаунтах
        {
            //SELECT * FROM test.accounts where w_password = '123' and w_email = '123';
            MySqlCommand command = new MySqlCommand(
                $"SELECT COUNT(*) FROM accounts WHERE w_password = '{password}' AND w_email = `{email}`;",
                connection);

            long count = (long)command.ExecuteScalar();
            Console.WriteLine($"SELECT COUNT(*) FROM accounts WHERE w_password = '{password}' AND w_email = `{email}`;");
            
            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            } 
        }

        public bool CheckEmail(string email) //Проверка почты в аккаунтах
        {
            MySqlCommand command = new MySqlCommand(
                $"SELECT COUNT(*) FROM accounts WHERE w_email = '{email}';",
                connection);
            long count = (long)command.ExecuteScalar();
            
            Console.WriteLine($"SELECT COUNT(*) FROM accounts WHERE w_email = '{email}';");
            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    public void AddAccount(string email, string password, string nick) //Добавить аккаунт
        {
            Console.WriteLine("3");
            MySqlCommand command = new MySqlCommand(
                $"INSERT INTO `accounts` (`w_email`, `w_password`, `w_nick`, `w_point`) VALUES ('{email}', '{password}', '{nick}', 0);",
                connection);
            Console.WriteLine($"INSERT INTO `accounts` (`w_email`, `w_password`, `w_nick`) VALUES ('{email}', '{password}', '{nick}');");
            
            command.ExecuteNonQuery();
            Console.WriteLine("3");
        }
    }
}