using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer
{
    public class PollAnswer
    {
        public string AnswerText { get; set; }
        public int Votes { get; set; }

        public PollAnswer(string answer)
        {
            AnswerText = answer;
            Votes = 0;
        }

        public PollAnswer(string answer, int votes)
        {
            AnswerText = answer;
            Votes = votes;
        }
    }
}
