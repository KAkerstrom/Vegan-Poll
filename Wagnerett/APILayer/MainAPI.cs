using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wagnerett {
    public partial class MainAPI : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            Response.ContentType = "text/html";
            object a = Request.Form;
            string ac = Request.Form["data[name]"].ToString();
            Response.Write(ac);
        }
    }
}
