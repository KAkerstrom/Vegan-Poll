using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace APILayer {
    public partial class MainAPI : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            //Get data from Request.Form in the form of "action", "data[PollQuestion]" or "data[doot[doot]]"
            Response.ContentType = "application/json";
            object a = Request.Form;
            string action = Request.Form["action"];

            switch (action)
            {
                //Server receives:
                //"action": "add_poll",
                //data: {
                //          "PollQuestion": (string),
                //          "EndDate": (datetime?),
                //          "AnswerType": (int),
                //          "Answers": [
                //              (string = answer text),
                //              (string = answer text),
                //              (string = answer text),
                //              (string = answer text)
                //          ]
                //}

                //Server sends:
                //"Success": (string = "true" or "false", if operation was successful),
                //"Error": [
                //          (string = error),
                //          (string = error),
                //          (string = error)
                //]
                //"PollID": (string),
                //"TripCode": (string)
                case "add_poll":
                    break;

                //Server receives:
                //"action": "close_poll",
                //data: (string = PollID)

                //Server sends:
                //"Success": (string = "true" or "false", if operation was successful),
                //"Error": [
                //          (string = error),
                //          (string = error),
                //          (string = error)
                //]
                case "close_poll":
                    break;

                //Server receives:
                //"action": "edit_poll",
                //data: {
                //          "PollID": (string),
                //          "PollQuestion": (string),
                //          "EndDate": (datetime?),
                //          "AnswerType": (int),
                //          "Answers": [
                //              (string = answer text),
                //              (string = answer text),
                //              (string = answer text),
                //              (string = answer text)
                //          ]
                //}

                //Server sends:
                //"Success": (string = "true" or "false", if operation was successful),
                //"Error": [
                //          (string = error),
                //          (string = error),
                //          (string = error)
                //]
                case "edit_poll":
                    break;

                //Server receives:
                //"action": "delete_poll",
                //(string = PollID)

                //Server sends:
                //"Success": (string = "true" or "false", if operation was successful),
                //"Error": [
                //          (string = error),
                //          (string = error),
                //          (string = error)
                //]
                case "delete_poll":
                    break;

                //Server receives:
                //"action": "vote",
                //data: (string = PollID),
                //      (string = Answer ID)

                //Server sends:
                //"Success": (string = "true" or "false", if operation was successful),
                //"Error": [
                //          (string = error),
                //          (string = error),
                //          (string = error)
                //]
                case "vote":
                    break;

                default:
                    break;
            }
        }
    }
}
