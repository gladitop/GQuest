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
                $"UPDATE accounts SET w_point = '{infoPoint.Point}' WHERE w_id = '{infoPoint.UserID}';",
                connection);             
            command.ExecuteNonQuery();
            Console.WriteLine($"Добавление в бд очки: id= {infoPoint.UserID}, points= {infoPoint.Point}");
        }
     
        public Data.ClientInfoOffile GetClientInfo(string email) //Получение инфо о клиенте
        {
            MySqlCommand command = new MySqlCommand(
                $"SELECT * FROM accounts WHERE w_email = '{email}';",
                connection);
            long id = 0;
            string emailInfo = "";
            string password = "";
            string nick = "";
            long? point = null;
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt64("w_id");
                emailInfo = reader.GetString("w_email");
                password = reader.GetString("w_password");
                nick = reader.GetString("w_nick");
                point = reader.GetInt64("w_point");//NULL!!!
            }
            reader.Close();
            return new Data.ClientInfoOffile(id, email, password, nick, point);
        }

        public bool CheckPassword(string Pemail,string Ppassword) //Проверка пароля в аккаунтах
        {
            Console.WriteLine($"Начало проверки пароля: {Pemail}");
            MySqlCommand command = new MySqlCommand(
                $"SELECT * FROM accounts WHERE w_email = '{Pemail}';",
                connection);
            string password = "";

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                password = reader.GetString("w_password");
            }
            reader.Close();  

            Console.WriteLine(Ppassword + " " + password);
            if(Ppassword == password) return true;
            else return false;          
        }

        public bool CheckEmail(string email) //Проверка почты в аккаунтах
        {
            Console.WriteLine($"Начало проверки почты: {email}");
            MySqlCommand command = new MySqlCommand(
                $"SELECT COUNT(*) FROM accounts WHERE w_email = '{email}';",
                connection);

                long count = (long)command.ExecuteScalar();
                                
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
            MySqlCommand command = new MySqlCommand(
                $"INSERT INTO `accounts` (`w_email`, `w_password`, `w_nick`, `w_point`) VALUES ('{email}', '{password}', '{nick}', 0);",
                connection);
            Console.WriteLine($"В БД добавился новый клиент: {email}, {password}, {nick}");
            command.ExecuteNonQuery();
        }
    }
}