using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DinamycServer
{
    public class Database
    {
        public MySqlConnection connection { get; set; }
        public Database()
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
    }
}