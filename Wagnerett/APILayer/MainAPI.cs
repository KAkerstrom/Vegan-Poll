using DBLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace APILayer
{
    public partial class MainAPI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Get data from Request.Form in the form of "action", "data[PollQuestion]" or "data[doot][index]"
            Response.ContentType = "application/json";
            string action = Request.Form["action"];
            string resp;

            switch (action)
            {
                //Server receives:
                //{
                //  "action":"add_poll",
                //  "data":{
                //            "PollQuestion": (string),
                //            "EndDate": (datetime?),
                //            "AnswerType": (int),
                //            "Answers": [
                //                (string = answer text),
                //                (string = answer text),
                //                (string = answer text),
                //                (string = answer text)
                //            ]
                //  }
                //}

                //Server sends:
                //{
                //  "Success":(string = "true" or "false", if operation was successful),
                //  "Error":[
                //            (string = error),
                //            (string = error),
                //            (string = error)
                //  ],
                //  "data":{
                //      "PollID":(string),
                //      "TripCode":(string)
                //   }
                //}
                case "add_poll":
                    {
                        Poll newPoll = new Poll();
                        newPoll.Question = Request.Form["data[PollQuestion]"];
                        newPoll.AnswerType = Convert.ToInt32(Request.Form["data[AnswerType]"]); //TryParse
                        newPoll.Answers = new List<PollAnswer>();

                        int answerIndex = 0;

                        //Testing
                        string answers = Request.Form[$"data[Answers][]"];
                        if (!string.IsNullOrEmpty(answers))
                            foreach (string s in answers.Split(','))
                                newPoll.Answers.Add(new PollAnswer(HexToUnicode(s)));

                        DateTime endDate;
                        if (DateTime.TryParse(Request.Form["data[EndDate]"], out endDate))
                            newPoll.EndDate = endDate;
                        else
                            newPoll.EndDate = null;

                        string id, trip;
                        if (DBInterface.CreatePoll(newPoll, out id, out trip))
                        {
                            resp = "{" +
                                          "\"Success\":true," +
                                          "\"Error\":[]," +
                                          "\"data\":{" +
                                          $"\"PollID\":\"{id}\"," +
                                          $"\"TripCode\":\"{trip}\"" +
                                          "}" +
                                          "}";
                            Response.Write(resp);
                        }
                        else
                        {
                            resp = "{" +
                                          "\"Success\":false," +
                                          "\"Error\":[]," + //Todo: Populate errors & add try-catches
                                          "\"data\":{" +
                                          $"\"PollID\":null," +
                                          $"\"TripCode\":null" +
                                          "}" +
                                          "}";
                            Response.Write(resp);
                        }
                    }
                    break;

                //Server receives:
                //{
                //  "action":"close_poll",
                //  "data":(string = PollID)
                //}

                //Server sends:
                //{
                //  "Success":(string = "true" or "false", if operation was successful),
                //  "Error":[
                //            (string = error),
                //            (string = error),
                //            (string = error)
                //  ]
                //}
                case "close_poll":
                    {
                        if (DBInterface.ClosePoll(Request.Form["data"]))
                        {
                            resp = "{" +
                                          "\"Success\":true," +
                                          "\"Error\":[]" +
                                          "}";
                            Response.Write(resp);
                        }
                        else
                        {
                            resp = "{" +
                                          "\"Success\":false," +
                                          "\"Error\":[]" + //Todo: Populate errors & add try-catches
                                          "}";
                            Response.Write(resp);
                        }
                    }
                    break;

                //Server receives:
                //{
                //  "action":"edit_poll",
                //  "data":{
                //            "PollID": (string),
                //            "PollQuestion": (string),
                //            "EndDate": (datetime?),
                //            "AnswerType": (int),
                //            "Answers": [
                //                (string = answer text),
                //                (string = answer text),
                //                (string = answer text),
                //                (string = answer text)
                //            ]
                //  }
                //}

                //Server sends:
                //  {
                //  "Success":(string = "true" or "false", if operation was successful),
                //  "Error":[
                //            (string = error),
                //            (string = error),
                //            (string = error)
                //  ]
                //}
                case "edit_poll":
                    {
                        Poll newPoll = new Poll();
                        newPoll.PollID = Request.Form["data[PollID]"];
                        newPoll.Question = Request.Form["data[PollQuestion]"];
                        newPoll.AnswerType = Convert.ToInt32(Request.Form["data[AnswerType]"]); //TryParse
                        newPoll.Answers = new List<PollAnswer>();
                        string[] answers = Request.Form["data[Answers]"].Split(',');
                        foreach (string answer in answers)
                            newPoll.Answers.Add(new PollAnswer(answer));

                        DateTime endDate;
                        if (DateTime.TryParse(Request.Form["data[EndDate]"], out endDate))
                            newPoll.EndDate = endDate;
                        else
                        {
                            //Todo: Throw a fit
                        }

                        string id, trip;
                        if (DBInterface.EditPoll(newPoll))
                        {
                            resp = "{" +
                                          "\"Success\":true," +
                                          "\"Error\":[]" +
                                          "}";
                            Response.Write(resp);
                        }
                        else
                        {
                            resp = "{" +
                                          "\"Success\":false," +
                                          "\"Error\":[]" + //Todo: Populate errors & add try-catches
                                          "}";
                            Response.Write(resp);
                        }
                    }
                    break;

                //Server receives:
                //{
                //  "action":"delete_poll",
                //  "data":(string = PollID)
                //}

                //Server sends:
                //{
                //  "Success":(string = "true" or "false", if operation was successful),
                //  "Error":[
                //            (string = error),
                //            (string = error),
                //            (string = error)
                //  ]
                //}
                case "delete_poll":
                    {
                        if (DBInterface.DeletePoll(Request.Form["data"]))
                        {
                            resp = "{" +
                                          "\"Success\":true," +
                                          "\"Error\":[]" +
                                          "}";
                            Response.Write(resp);
                        }
                        else
                        {
                            resp = "{" +
                                          "\"Success\":false," +
                                          "\"Error\":[]" + //Todo: Populate errors & add try-catches
                                          "}";
                            Response.Write(resp);
                        }
                    }
                    break;

                //Server receives:
                //{
                //  "action":"vote",
                //  "data":{
                //       "PollID":(string = PollID),
                //       "AnswerID":(string = Answer ID)
                //  }
                //}

                //Server sends:
                //{
                //  "Success":(string = "true" or "false", if operation was successful),
                //  "Error":[
                //            (string = error),
                //            (string = error),
                //            (string = error)
                //  ]
                //}
                case "vote":
                    {
                        string id = Request.Form["data[PollID]"];
                        int answerId;
                        if (!string.IsNullOrEmpty(Request.Form["data[AnswerID]"]))
                        {
                            answerId = Convert.ToInt32(Request.Form["data[AnswerID]"]);

                            if (DBInterface.Vote(id, answerId))
                            {
                                resp = "{" +
                                       "\"Success\":true," +
                                       "\"Error\":[]" +
                                       "}";
                                Response.Write(resp);
                            }
                            else
                            {
                                resp = "{" +
                                       "\"Success\":false," +
                                       "\"Error\":[]" + //Todo: Populate errors & add try-catches
                                       "}";
                                Response.Write(resp);
                            }
                        }
                        else
                        {
                            resp = "{" +
                                   "\"Success\":false," +
                                   "\"Error\":[]" + //Todo: Populate errors & add try-catches
                                   "}";
                            Response.Write(resp);
                        }
                    }
                    break;

                //Server receives:
                //{
                //  "action":"get_poll",
                //  "data":{ "PollID":(string = PollID) }
                //}

                //Server sends:
                //"Success":(string = "true" or "false", if operation was successful),
                //"Error":[
                //            (string = error),
                //            (string = error),
                //            (string = error)
                //  ]
                //"Poll":{
                //  "PollQuestion": (string),
                //  "EndDate": (datetime?),
                //  "AnswerType": (int),
                //  "AnswerCount": (int),
                //  "Answers": [
                //      { "ID": (int), "Text": (string), "Votes": (int) },
                //      { "ID": (int), "Text": (string), "Votes": (int) },
                //      { "ID": (int), "Text": (string), "Votes": (int) }
                //  ]
                //}
                case "get_poll":
                    Poll poll = DBInterface.GetPoll(Request.Form["data[PollID]"]);
                    if (poll != null)
                    {
                        StringBuilder sb = new StringBuilder("{\"Success\":true,\"Error\":[],");
                        sb.Append($"\"Poll\":{PollToJson(poll)}}}");
                        resp = sb.ToString();
                    }
                    else
                        resp = $"{{\"Success\":false,\"Error\":[\"Poll #{Request.Form["data[PollID]"]} was not found.\"]}}";

                    Response.Write(resp);
                    break;

                //Server receives:
                //{
                //  "action":"get_poll_history",
                //  "data":{ "PollCount":(int = Number of polls to get) }
                //}

                //Server sends:
                //"Success":(string = "true" or "false", if operation was successful),
                //"Error":[
                //            (string = error),
                //            (string = error),
                //            (string = error)
                //  ]
                //"Polls":[
                //  {
                //     "PollID": (string),
                //     "PollQuestion": (string),
                //     "EndDate": (datetime?),
                //     "AnswerType": (int),
                //     "AnswerCount": (int),
                //     "Answers": [
                //         { "ID": (int), "Text": (string), "Votes": (int) },
                //         { "ID": (int), "Text": (string), "Votes": (int) },
                //         { "ID": (int), "Text": (string), "Votes": (int) }
                //     ]
                //  },
                //  (etc...)
                //]
                case "get_poll_history":
                    List<Poll> polls = DBInterface.GetRecentPolls(int.Parse(Request.Form["data[PollCount]"]));
                    if (polls != null)
                    {
                        StringBuilder sb = new StringBuilder("{\"Success\":true,\"Error\":[], ");
                        sb.Append("\"Polls\":[");
                        foreach (Poll topPoll in polls)
                        {
                            sb.Append(PollToJson(topPoll) + ",");
                        }

                        if (polls.Count > 0)
                            sb.Remove(sb.Length - 1, 1);
                        sb.Append("]}");

                        resp = sb.ToString();
                    }
                    else
                        resp = $"{{\"Success\":false,\"Error\":[\"Poll #{Request.Form["data[PollID]"]} was not found.\"]}}";

                    Response.Write(resp);
                    break;

                default:
                    resp = $"{{\"Success\":false,\"Error\":[\"Invalid command: '{action}'\"]}}";
                    break;
            }
        }

        private string HexToUnicode(string hex)
        {
            if (hex.Length % 4 != 0)
                throw new Exception("Length of hex string was not divisible by 4.");

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length / 2; i++)
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);

            Encoding unicode = Encoding.BigEndianUnicode;
            return unicode.GetString(bytes);
        }

        /// <summary>
        /// This should really be done with Json.Net, but it works in a pinch.
        /// </summary>
        /// <param name="poll">The poll to convert.</param>
        /// <returns>A Json string of the poll.</returns>
        private string PollToJson(Poll poll)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append($"\"PollID\": {JsonConvert.ToString(poll.PollID)},");
            sb.Append($"\"PollQuestion\": {JsonConvert.ToString(poll.Question)},");
            sb.Append($"\"EndDate\": {JsonConvert.ToString(poll.EndDate)},");
            sb.Append($"\"AnswerType\": {JsonConvert.ToString(poll.AnswerType)},");
            sb.Append("\"Answers\":[");
            foreach (PollAnswer answer in poll.Answers)
                sb.Append($"{{\"ID\": {JsonConvert.ToString(answer.AnswerID)}, \"Text\": {JsonConvert.ToString(answer.AnswerText)}, \"Votes\": {JsonConvert.ToString(answer.Votes)}}},");
            if (poll.Answers.Count > 0)
                sb.Remove(sb.Length - 1, 1); //Remove trailing comma
            sb.Append("]}");
            return sb.ToString();
        }
    }
}
