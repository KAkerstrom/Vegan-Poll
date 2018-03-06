using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer
{
    public class Poll
    {
        public string Question { get; set; }
        public List<PollAnswer> Answers { get; set; }
    }
}
