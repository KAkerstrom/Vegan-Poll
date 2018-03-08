using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DBLayer
{
    class DBInterface
    {
        private const string Server = "172.18.28.12";
        private const string Database = "Test";
        private const string UserName = "SQLAdmin";
        private const string Password = "P@$$W0rd";

        /// <summary>
        /// Generates a 32-character hexadecimal GUID string.
        /// </summary>
        /// <returns>A 32-character GUID.</returns>
        private static string GetRandomCode()
        {
            Guid id = Guid.NewGuid();
            return id.ToString().Replace("-", "");
        }

        /// <summary>
        /// Creates a poll in the database.
        /// </summary>
        /// <param name="poll">The poll to insert into the database.</param>
        /// <returns>Returns whether the operation was successful.</returns>
        public static bool CreatePoll(Poll poll)
        {
            SqlConnection con = new SqlConnection();

            string conString = $"Server={Server};Database={Database};User Id={UserName};Password={Password};Connection Timeout=5;";
            string sqlString =
                "INSERT INTO Polls (PollID, PollQuestion, TimeCreated, EndDate, TripCode, AnswerTypeID) VALUES @pollId, @pollQuestion, @timeCreated, @endDate, @tripCode, @answerTypeId)";

            con.ConnectionString = conString;

            SqlCommand command = new SqlCommand(sqlString, con);
            command.Parameters.AddWithValue("@pollId", poll.PollID);
            command.Parameters.AddWithValue("@pollQuestion", poll.Question);
            command.Parameters.AddWithValue("@timeCreated", DateTime.Now);
            command.Parameters.AddWithValue("@endDate", poll.EndDate);
            command.Parameters.AddWithValue("@tripCode", poll.Tripcode);
            //command.Parameters.AddWithValue("@answerTypeId", poll.PUTANANSWERTYPEHERE);
            command.Parameters.AddWithValue("@pollQuestion", poll.Question);

            //Also remember to insert poll answers!

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

        public static Poll GetPoll(int PollID)
        {
            string conString = $"Server={Server};Database={Database};User Id={UserName};Password={Password};Connection Timeout=5;";
            string sql = "SELECT * FROM Polls WHERE PollID = @pID";
            Poll retPoll = new Poll();

            SqlConnection con = new SqlConnection(conString);
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.Add("@pID", SqlDbType.Int);
            command.Parameters["@pID"].Value = PollID;

            using (SqlDataReader dr = command.ExecuteReader())
            {
                while (dr.Read())
                {
                    string treatment = dr[0].ToString();
                }
            }

            return retPoll;
        }
            

    }
}
