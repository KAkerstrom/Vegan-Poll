using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer
{
    public class PollAnswer
    {
        public int AnswerID { get; set; }
        public string PollID { get; set; }
        public string AnswerText { get; set; }
        public int Votes { get; set; }
    }
}
