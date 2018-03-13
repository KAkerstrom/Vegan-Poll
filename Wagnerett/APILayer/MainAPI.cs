using DBLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace APILayer
{
    public partial class MainAPI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Get data from Request.Form in the form of "action", "data[PollQuestion]" or "data[doot[doot]]"
            Response.ContentType = "application/json";
            string action = Request.Form["action"];

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
                        if (DBInterface.CreatePoll(newPoll, out id, out trip))
                        {
                            string resp = "{" +
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
                            string resp = "{" +
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
                //  data:(string = PollID)
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
                            string resp = "{" +
                                          "\"Success\":true," +
                                          "\"Error\":[]" +
                                          "}";
                            Response.Write(resp);
                        }
                        else
                        {
                            string resp = "{" +
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
                //  data:{
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
                            string resp = "{" +
                                          "\"Success\":true," +
                                          "\"Error\":[]" +
                                          "}";
                            Response.Write(resp);
                        }
                        else
                        {
                            string resp = "{" +
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
                            string resp = "{" +
                                          "\"Success\":true," +
                                          "\"Error\":[]" +
                                          "}";
                            Response.Write(resp);
                        }
                        else
                        {
                            string resp = "{" +
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
                //  data:{
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
                    string answer = Request.Form["data[AnswerID]"];
                    if (DBInterface.Vote(id, answer))
                    {
                        string resp = "{" +
                                      "\"Success\":true," +
                                      "\"Error\":[]" +
                                      "}";
                        Response.Write(resp);
                    }
                    else
                    {
                        string resp = "{" +
                                      "\"Success\":false," +
                                      "\"Error\":[]" + //Todo: Populate errors & add try-catches
                                      "}";
                        Response.Write(resp);
                    }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
