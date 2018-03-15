using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer
{
    public class Poll
    {
        public string PollID { get; set; }
        public string Question { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? EndDate { get; set; }
        public string Tripcode { get; set; }
        public int AnswerType { get; set; }
        public bool Disabled { get; set; }
        public List<PollAnswer> Answers { get; set; }

        public Poll(string ID, string Quest, DateTime Datecreate, DateTime? End, string trip, int Answer)
        {
            PollID = ID;
            Question = Quest;
            DateCreated = Datecreate;
            EndDate = End;
            Tripcode = trip;
            AnswerType = Answer;
            Disabled = false;
        }
        public Poll(string ID, string Quest, DateTime Datecreate, DateTime? End, string trip, int Answer, bool dis)
        {
            PollID = ID;
            Question = Quest;
            DateCreated = Datecreate;
            EndDate = End;
            Tripcode = trip;
            AnswerType = Answer;
            Disabled = dis;
        }
        public Poll()
        {

        }
    }
}
