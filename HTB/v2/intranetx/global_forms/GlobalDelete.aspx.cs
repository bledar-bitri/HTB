using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using HTB.Database;

namespace HTB.v2.intranetx.global_forms
{
    public partial class GlobalDelete : System.Web.UI.Page
    {
        string id = "";
        string table = "";
        string column = "";
        string question = "";
        string textField = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            bdy.Attributes.Add("onload", "movesizeandopen();");

            id = Request.QueryString["ID"];
            if (id != null && !id.Trim().Equals(string.Empty))
            {
                table = Request.QueryString["strTable"];
                column = Request.QueryString["strColumn"];
                question = Request.QueryString["frage"];
                textField = Request.QueryString["strTextField"];
                SetValues();
            }
        }

        private void SetValues()
        {
            lblQuestion.Text = question;
            lblTextField.Text = GetDescription();
        }

        private string GetDescription(int connectToDatabase = DatabasePool.ConnectToHTB)
        {
            string sqlCommand = "SELECT *  FROM " + table + " WHERE " + column + " = " + id;
            SqlDataReader Results;
            DbConnection con = DatabasePool.GetConnection(sqlCommand, connectToDatabase);
            SqlCommand cmd = new SqlCommand(sqlCommand, con.Connection);
            Results = cmd.ExecuteReader();
            if (Results.Read())
            {
                if (Results[textField] != null)
                    return Results[textField].ToString();
            }
            Results.Close();
            con.IsInUse = false;
            return "";
        }

        private void DoDelete(int connectToDatabase = DatabasePool.ConnectToHTB)
        {
            string sqlCommand = "DELETE FROM " + table + " WHERE " + column + " = " + id;
            DbConnection con = DatabasePool.GetConnection(sqlCommand, connectToDatabase);
            SqlCommand cmd = new SqlCommand(sqlCommand, con.Connection);
            cmd.ExecuteNonQuery();
            con.IsInUse = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DoDelete();
            CloseWindowAndRefresh();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseWindow();
        }

        private void CloseWindowAndRefresh()
        {
            bdy.Attributes.Add("onload", "MM_refreshParentAndClose();");
        }

        private void CloseWindow()
        {
            bdy.Attributes.Add("onload", "window.close();");
        }
    }
}