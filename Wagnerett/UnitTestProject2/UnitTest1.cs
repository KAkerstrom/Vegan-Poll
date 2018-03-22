using System;
using DBLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        string hmmmm;
        static string haha = "450";
        static DateTime? NANI = DateTime.Now;
        static DateTime WHAT = DateTime.Now;

        Poll p = new Poll(haha, "Why even try?", WHAT, null, "", 1);
        Poll pr;
        [TestMethod]
        public void CreatePoll()
        {

          
          
            DBInterface.CreatePoll(p, out string sad, out string meh);
            
        }
        [TestMethod]
        public void GetPoll()
        {
            DBInterface.CreatePoll(p, out string sad, out string meh);
            pr = DBInterface.GetPoll(sad);
            Assert.IsNotNull(pr);
            Assert.AreEqual(p.Question, pr.Question);
            Assert.AreEqual(p.Disabled, pr.Disabled);
            Assert.AreEqual(p.Tripcode, pr.Tripcode);
           // Assert.AreEqual(p.EndDate, pr.EndDate);
            Assert.AreEqual(p.AnswerType, pr.AnswerType);
           //Assert.AreEqual(p.DateCreated, pr.DateCreated);
            Assert.AreEqual(p.PollID, pr.PollID);
            Assert.AreEqual(p.Answers, pr.Answers);
            
        }
        [TestMethod]
        public void ClosePoll()
        {
            bool cp = true,cpp;
            DBInterface.CreatePoll(p, out string sad, out string meh);
            cpp = DBInterface.ClosePoll(sad);
            Assert.AreEqual(cp, cpp);
        }
    }
}
