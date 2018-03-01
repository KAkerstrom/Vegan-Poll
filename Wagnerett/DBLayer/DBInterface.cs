using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer
{
    class DBInterface
    {
        private const string Server = "172.18.28.12";
        private const string Database = "Test";
        private const string UserName = "SQLAdmin";
        private const string Password = "P@$$W0rd";

        private static string GetRandomCode()
        {
            throw new NotImplementedException();
        }

        public static bool CreatePoll(Poll poll)
        {
            SqlConnection con = new SqlConnection();

            string conString =
                $"Server={Server};Database={Database};User Id={UserName};Password={Password};Connection Timeout=5;";
            string sqlString = "INSERT INTO Polls ()";

            con.ConnectionString = conString;

            try
            {
                con.Open();

                con.Close();
            }
            catch (SqlException sqlex)
            {
                return false;
            }

            return true;
        }
    }
}
