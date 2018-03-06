using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;

namespace Wagnerett
{
    public partial class Dustin_ssupertechywebform : System.Web.UI.Page
    {
        private const string ServerIP = "172.18.28.12";
        private const int gridcol0 = 0;
        private const string Serverusername = "SQLAdmin";
        private const string ServerPass = "P@$$W0rd";// shhhhhhh don't look
        private const string ServerDB = "Test";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                ReadDataFromDB();
           
        }

        protected void ReadDataFromDB()
        {
           
            DataTable dt = new DataTable();
            dt.Columns.Add("Test");
            //172.18.28.12 derterbers  P@$$W0rd
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString =
                "Data Source=172.18.28.12:8080;" +
                "Initial Catalog=Test;" +
                "User id=SQLAdmin;" +
                "Password=P@$$W0rd;";
           // conn.Open();
            string SQLString = "SELECT * FROM Test";
            OleDbConnection Con = new OleDbConnection(conn.ConnectionString);
            OleDbCommand Cmd = new OleDbCommand(SQLString, Con);

            Cmd.Connection.Open();
            OleDbDataReader myReader = Cmd.ExecuteReader();

            while (myReader.Read())
            {
                DataRow dr = dt.NewRow();
                dr["Test"] = myReader.GetString(1);
              
                dt.Rows.Add(dr);
            }
            myReader.Close();
            Con.Close();

            GridView1.DataSource = dt;
            GridView1.DataBind();

    

        }
    }
}