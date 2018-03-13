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
    public static class DBInterface
    {
        private const string Server = "172.18.28.12";
        private const string Database = "Vegan-Poll";
        private const string UserName = "SQLAdmin";
        private const string Password = "P@$$W0rd";

        private static Dictionary<int, string> AnswerTypeDictionary = new Dictionary<int, string>();
        private static SqlConnection con;

        static DBInterface()
        {
            TestDB();
            string sql = "SELECT * FROM AnswerTypes";
            OpenDB();
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AnswerTypeDictionary.Add((int) reader["AnswerTypeID"], (string) reader["Type"]);
            }

            CloseDB();
        }

        /// <summary>
        /// Opens and closes database to check for connection problems. May be redundant, should be safe to remove.
        /// </summary>
        private static void TestDB()
        {
            OpenDB();
            CloseDB();
        }


        /// <summary>
        /// Attempts to create and open database connection.
        /// </summary>
        private static void OpenDB()
        {
            if (con == null)
            {
                con = new SqlConnection(
                    $"Server={Server};Database={Database};User Id={UserName};Password={Password};Connection Timeout=5;");
            }

            if (con.State != System.Data.ConnectionState.Open)
            {
                con.Open();
            }
        }

        /// <summary>
        /// Checks if connection exists and is open, then attempts to close database connection.
        /// </summary>
        private static void CloseDB()
        {
            if (con != null && con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }

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
        public static bool CreatePoll(Poll poll, out string pollID, out string tripCode)
        {
            pollID = GetRandomCode();
            tripCode = GetRandomCode();
            poll.PollID = pollID;
            poll.Tripcode = tripCode;

            //TODO: finish this method
            string sqlString =
                "INSERT INTO Polls (PollID, PollQuestion, TimeCreated, EndDate, TripCode, AnswerTypeID) VALUES (@pollId, @pollQuestion, @timeCreated, @endDate, @tripCode, @answerTypeId)";
            SqlCommand command = new SqlCommand(sqlString, con);
            command.Parameters.AddWithValue("@pollId", poll.PollID);
            command.Parameters.AddWithValue("@pollQuestion", poll.Question);
            command.Parameters.AddWithValue("@timeCreated", DateTime.Now);
            command.Parameters.AddWithValue("@endDate", NullToDBNull(poll.EndDate));
            command.Parameters.AddWithValue("@tripCode", poll.Tripcode);
            command.Parameters.AddWithValue("@answerTypeId", poll.AnswerType);
            //command.Prepare();
            //Also remember to insert poll answers!

            try
            {
                OpenDB();
                command.ExecuteNonQuery();
                CloseDB();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public static void InsertPollAnswer(List<PollAnswer> AnswerList)
        {
            foreach (PollAnswer A in AnswerList)
            {
                InsertPollAnswer(A);
            }
        }

        public static void InsertPollAnswer(PollAnswer Answer)
        {
            string sqlString =
                "INSERT INTO PollAnswers (AnswerID, PollID, AnswerText, AnswerCount) VALUES @answerID, @pollID, @answerText, @answerCount)";
            SqlCommand command = new SqlCommand(sqlString, con);
            command.Parameters.AddWithValue("@answerID", Answer.AnswerID);
            command.Parameters.AddWithValue("@pollID", Answer.PollID);
            command.Parameters.AddWithValue("@answerText", Answer.AnswerText);
            command.Parameters.AddWithValue("@answerCount", Answer.Votes);

            try
            {
                OpenDB();
                command.ExecuteNonQuery();
                CloseDB();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static Poll GetPoll(int PollID)
        {
            string sql = "SELECT * FROM Polls WHERE PollID = @pID";
            Poll retPoll = new Poll();
            OpenDB();
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

            CloseDB();
            return retPoll;
        }

        public static bool ClosePoll(string pollID)
        {
            string sql = "UPDATE Polls SET EndDate = @eDate WHERE PollID = @pID";
            SqlCommand command = new SqlCommand(sql, con);
            try
            {
                int pID = Convert.ToInt32(pollID);
                command.Parameters.AddWithValue("@eDate", DateTime.Now);
                command.Parameters.AddWithValue("@pID", pID);
                OpenDB();
                command.ExecuteNonQuery();
                CloseDB();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool EditPoll(Poll newPoll)
        {
            return true;
        }

        public static bool DeletePoll(string pollID)
        {
            return true;
        }

        public static bool Vote(string pollID ,string answer)
        {
            return true;
        }
        /// <summary>
        /// Replaces null Object with DBNull.Value. Used for adding nullable types to database.
        /// </summary>
        /// <param name="O"></param>
        /// <returns></returns>
        private static Object NullToDBNull(Object O)
        {
            return O ?? DBNull.Value;
        }
        /// <summary>
        /// Rebleces DBNull Objects with null. Used for translating null database entries to nullable parameters
        /// </summary>
        /// <param name="O"></param>
        /// <returns></returns>
        private static Object DBNullToNull(Object O)
        {
            return O == DBNull.Value ? null : O;
        }
    }
}
