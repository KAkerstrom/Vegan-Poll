using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer
{
    public static class DBInterface
    {
        //private const string Server = "172.18.28.12";
        private const string Server = "10.0.1.40";
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

            //Loads answer type names and IDs from database
            while (reader.Read())
            {
                AnswerTypeDictionary.Add((int)reader["AnswerTypeID"], (string)reader["Type"]);
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

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
        }

        /// <summary>
        /// Checks if connection exists and is open, then attempts to close database connection.
        /// </summary>
        private static void CloseDB()
        {
            if (con != null && con.State == ConnectionState.Open)
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

            if (poll == null || poll.Answers == null)
                return false; //Pass error as string??

            //TODO: finish this method
            string sqlString = "INSERT INTO Polls (PollID, PollQuestion, TimeCreated, EndDate, TripCode, AnswerTypeID, Disabled) VALUES (@pollId, @pollQuestion, @timeCreated, @endDate, @tripCode, @answerTypeId, @disabled)";
            SqlCommand command = new SqlCommand(sqlString, con);
            command.Parameters.AddWithValue("@pollId", poll.PollID);
            command.Parameters.AddWithValue("@pollQuestion", poll.Question);
            command.Parameters.AddWithValue("@timeCreated", DateTime.Now);
            command.Parameters.AddWithValue("@endDate", NullToDBNull(poll.EndDate));
            command.Parameters.AddWithValue("@tripCode", poll.Tripcode);
            command.Parameters.AddWithValue("@answerTypeId", poll.AnswerType);
            command.Parameters.AddWithValue("@disabled", poll.Disabled);

            for (int i = 0; i < poll.Answers.Count; i++)
            {
                poll.Answers[i].PollID = poll.PollID;
                poll.Answers[i].AnswerID = i + 1;
            }


            try
            {
                OpenDB();
                command.ExecuteNonQuery();
                CloseDB();
                InsertPollAnswer(poll.Answers);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when adding poll to the database.");
                return false;
            }

            return true;
        }

        public static bool InsertPollAnswer(List<PollAnswer> AnswerList)
        {
            foreach (PollAnswer A in AnswerList)
            {
                if (!InsertPollAnswer(A))
                    return false;
            }

            return true;
        }

        public static bool InsertPollAnswer(PollAnswer Answer)
        {
            string sqlString = "INSERT INTO PollAnswers (AnswerID, PollID, AnswerText, AnswerCount) VALUES (@answerID, @pollID, @answerText, @answerCount)";
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
                return false;
            }

            return true;
        }

        public static Poll GetPoll(string PollID)
        {
            string sql = "SELECT * FROM Polls WHERE PollID = @pID";
            Poll poll = new Poll();
            OpenDB();
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@pID", PollID);
            SqlDataReader dr = null;

            try
            {
                dr = command.ExecuteReader();
                while (dr.Read()) {
                    poll.PollID = dr["PollID"].ToString();
                    poll.Question = dr["PollQuestion"].ToString();
                    poll.DateCreated = Convert.ToDateTime(dr["TimeCreated"]);
                    poll.EndDate = dr["EndDate"] == DBNull.Value ? null : (DateTime?)dr["EndDate"];
                    poll.Tripcode = dr["Tripcode"].ToString();
                    poll.AnswerType = Convert.ToInt32(dr["AnswerTypeID"]);
                    poll.Disabled = Convert.ToBoolean(dr["Disabled"]);
                }
                dr.Close();

                sql = "SELECT AnswerID, PollID, AnswerText, AnswerCount FROM PollAnswers WHERE PollID = @pId";
                command = new SqlCommand(sql, con);
                command.Parameters.AddWithValue("@pId", poll.PollID);
                dr = command.ExecuteReader();
                while (dr.Read())
                    poll.Answers.Add(new PollAnswer((string)dr["PollID"], (int)dr["AnswerID"], (string)dr["AnswerText"], (int)dr["AnswerCount"]));
                dr.Close();

                CloseDB();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting poll: " + ex.Message);
                return null;
            }
            finally {
                if (dr != null && !dr.IsClosed)
                    dr.Close();
            }

            return poll;
        }

        public static List<Poll> GetRecentPolls(int pollCount)
        {
            string sql = $"SELECT TOP {pollCount} * FROM Polls WHERE (EndDate > GETDATE() OR EndDate IS NULL) AND Disabled = 0 ORDER BY TimeCreated DESC";
            List<Poll> topPolls = new List<Poll>();
            SqlDataReader dr = null;
            try
            {
                OpenDB();
                SqlCommand command = new SqlCommand(sql, con);

                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    Poll poll = new Poll();
                    poll.PollID = dr["PollID"].ToString();
                    poll.Question = dr["PollQuestion"].ToString();
                    poll.DateCreated = Convert.ToDateTime(dr["TimeCreated"]);
                    poll.EndDate = dr["EndDate"] == DBNull.Value ? null : (DateTime?)dr["EndDate"];
                    poll.Tripcode = dr["Tripcode"].ToString();
                    poll.AnswerType = Convert.ToInt32(dr["AnswerTypeID"]);
                    poll.Disabled = Convert.ToBoolean(dr["Disabled"]);
                    topPolls.Add(poll);
                }
                dr.Close();

                foreach (Poll p in topPolls) {
                    sql = "SELECT AnswerID, PollID, AnswerText, AnswerCount FROM PollAnswers WHERE PollID = @pId";
                    command = new SqlCommand(sql, con);
                    command.Parameters.AddWithValue("@pId", p.PollID);
                    dr = command.ExecuteReader();
                    while (dr.Read())
                        p.Answers.Add(new PollAnswer((string)dr["PollID"], (int)dr["AnswerID"], (string)dr["AnswerText"], (int)dr["AnswerCount"]));
                    dr.Close();
                }

                CloseDB();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting recent polls: " + ex.Message);
                return null;
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                    dr.Close();
            }

            return topPolls;
        }

        public static bool ClosePoll(string pollID)
        {
            string sql = "UPDATE Polls SET EndDate = @eDate WHERE PollID = @pID";
            SqlCommand command = new SqlCommand(sql, con);
            try
            {
                command.Parameters.AddWithValue("@eDate", DateTime.Now);
                command.Parameters.AddWithValue("@pID", pollID);
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

        /// <summary>
        /// Updates a poll in the database that has the same ID as the poll taken as a parameter. PollID, TimeCreated, and Tripcode are not changed.
        /// </summary>
        /// <param name="newPoll"></param>
        /// <returns></returns>
        public static bool EditPoll(Poll newPoll)
        {
            string sql = "UPDATE Polls SET PollQuestion = @pQuestion, EndDate = @pEndDate, AnswerTypeID = @pAnswerTypeID WHERE PollID = @pID";
            SqlCommand command = new SqlCommand(sql, con);
            try
            {
                command.Parameters.AddWithValue("@pQuestion", newPoll.Question);
                command.Parameters.AddWithValue("@pEndDate", NullToDBNull(newPoll.EndDate));
                command.Parameters.AddWithValue("@AnswerTypeID", newPoll.AnswerType);
                command.Parameters.AddWithValue("@pID", newPoll.PollID);
                OpenDB();
                int affectedRows = command.ExecuteNonQuery();
                CloseDB();

                if (affectedRows == 1)
                {
                    return true;
                }

                if (affectedRows == 0)
                {
                    throw new Exception("Poll does not exist in database.");
                }

                throw new Exception("ERROR: Multiple rows affected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool DeletePoll(string pollID)
        {
            string sql = "UPDATE Polls SET Disabled = 1 WHERE PollID = @pID";
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@pID", pollID);
            OpenDB();
            int affectedRows = command.ExecuteNonQuery();
            CloseDB();

            if (affectedRows == 1)
            {
                return true;
            }

            if (affectedRows == 0)
            {
                throw new Exception("Poll does not exist in database.");
            }

            throw new Exception("ERROR: Multiple rows affected.");
        }

        /// <summary>
        /// Increases the vote count on the given poll answer.
        /// </summary>
        /// <param name="pollId">The Poll ID.</param>
        /// <param name="answerId">The Answer ID.</param>
        /// <returns>Returns true if successful. If not, writes the error to the console and returns false.</returns>
        public static bool Vote(string pollId, int answerId)
        {
            string sql = "UPDATE PollAnswers SET AnswerCount = AnswerCount + 1 WHERE PollID = @pollId and AnswerID = @answerId";
            SqlCommand command = new SqlCommand(sql, con);
            try
            {
                command.Parameters.AddWithValue("@pollId", pollId);
                command.Parameters.AddWithValue("@answerId", answerId);
                OpenDB();
                int rowsChanged = command.ExecuteNonQuery();
                CloseDB();
                if (rowsChanged != 1)
                    return false;
                    //throw new Exception($"Update statement updated {rowsChanged} rows when placing vote.");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
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
