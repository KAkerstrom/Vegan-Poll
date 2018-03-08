using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer
{
    public enum AnswerTypes
    {
        Single, Multiple
    }

    public class Poll
    {
        public string PollID { get; set; }
        public string Question { get; set; }
        public DateTime DateCreated { get; }
        public DateTime EndDate { get; set; }
        public string Tripcode { get; set; }
        public AnswerTypes AnswerType { get; set; }
        public List<PollAnswer> Answers { get; set; }
    }
}
