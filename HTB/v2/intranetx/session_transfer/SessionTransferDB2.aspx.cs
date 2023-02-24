using System;
using System.Collections;
using System.Web;
using System.Data.SqlClient;
using HTB.Database;
using HTB.v2.intranetx.util;
//using IBM.Data.DB2;

namespace HTB.session_transfer
{
	/// <summary>
	/// Summary description for SessionTransfer.
	/// </summary>
    public partial class SessionTransferDB2 : System.Web.UI.Page
	{
	    private static string _connString = System.Configuration.ConfigurationManager.AppSettings["DB2RoadsConnectionString"];

		protected void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                Response.Cache.SetExpires(DateTime.Parse(DateTime.Now.ToString()));
                Response.Cache.SetCacheability(HttpCacheability.Private);
                string guidSave;
                Response.Write("HERE!!!");

                if (Request.QueryString["dir"] == "2asp")
                {
                    //Add the session information to the database, and redirect to the
                    //  ASP implementation of SessionTransfer.
                    guidSave = AddSessionToDatabase();
                    Response.Redirect("SessionTransfer.asp?dir=2asp&guid=" + guidSave +
                                      "&url=" + Server.UrlEncode(Request.QueryString["url"]));
                }
                else
                {
                    //Retreive the session information, and redirect to the URL specified
                    //  by the querystring.
                    GetSessionFromDatabase(Request.QueryString["guid"]);
                    Response.Write("HERE!");
                    ClearSessionFromDatabase(Request.QueryString["guid"]);
                    Response.Redirect(GlobalUtilArea.DecodeUrl(Request.QueryString["url"]));

                    Response.Write("GUID: " + Request.QueryString["guid"] + "<br>");
                    Response.Write("URL:  " + Request.QueryString["url"] + "<br>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<br><br>"+ex.Message+"<BR/><BR>"+ex.StackTrace);
            }		}


		//This method adds the session information to the database and returns the GUID
		//  used to identify the data.
        private string AddSessionToDatabase()
        {

            var set = new RecordSet();
            string strSql, guidTemp = GetGuid();
            int i = 0;
            try
            {
                while (i < Session.Contents.Count)
                {
                    strSql = "INSERT INTO tblASPSessionState (GUID, SessionKey, SessionValue) " +
                             "VALUES ('" + guidTemp + "', '" + Session.Contents.Keys[i].ToString() + "', '" +
                             Session.Contents[i].ToString() + "')";
                    set.ExcecuteNonQueryInTransaction(strSql);
                    i++;
                }
                set.CommitTransaction();
            }
            catch
            {
                set.RollbackTransaction();
                return null;
            }
            return guidTemp;
        }

        
        private string AddSessionToDatabaseOld()
		{

			//**************************************
			//Enter connection information here

            var con = new SqlConnection(_connString);
			
			//**************************************

			var cmd = new SqlCommand();
			con.Open();
			cmd.Connection = con;
			int i = 0;
			string strSql, guidTemp = GetGuid();
			
			while (i < Session.Contents.Count)
			{
				strSql = "INSERT INTO tblASPSessionState (GUID, SessionKey, SessionValue) " + 
					"VALUES ('" + guidTemp + "', '" + Session.Contents.Keys[i].ToString() + "', '" + 
					Session.Contents[i].ToString() + "')";
				cmd.CommandText = strSql;
				cmd.ExecuteNonQuery();
				i++;
			}

			con.Close();
			cmd.Dispose();
			con.Dispose();

			return guidTemp;
		}


		//This method retrieves the session information identified by the parameter
		//  guidIn from the database.
		private void GetSessionFromDatabase(string guidIn)
		{
            //Get a DataReader that contains all the Session information
            ArrayList list = HTBUtilities.HTBUtils.GetSqlRecords("SELECT * FROM tblASPSessionState WHERE GUID = '" + guidIn + "'", typeof(tblASPSessionState));
			

			//Iterate through the results and store them in the session object
			foreach (tblASPSessionState rec in list)
			{
				Session[rec.SessionKey] = rec.SessionValue;
			}
		}

        private void GetSessionFromDatabaseX(string guidIn)
        {
            //Get a DataReader that contains all the Session information
            /*
            var con = new DB2Connection(_connString);
            con.Open();
            var cmd = new DB2Command("SELECT * FROM tblASPSessionState WHERE GUID = '" + guidIn + "'", con);
            
            DB2DataReader dr = cmd.ExecuteReader();

            //Iterate through the results and store them in the session object
            while (dr.Read())
            {
                Session[dr["SessionKey"].ToString()] = dr["SessionValue"].ToString();
            }
            dr.Close();
            con.Close();
            dr.Dispose();
            con.Dispose();
             */
        }

        private void GetSessionFromDatabaseOld(string guidIn)
        {
            //**************************************
            //Enter connection information here

            var con = new SqlConnection(_connString);

            //**************************************
            var cmd = new SqlCommand();
            SqlDataReader dr;
            con.Open();
            cmd.Connection = con;

            //Get a DataReader that contains all the Session information
            string strSql = "SELECT * FROM tblASPSessionState WHERE GUID = '" + guidIn + "'";
            cmd.CommandText = strSql;
            dr = cmd.ExecuteReader();

            //Iterate through the results and store them in the session object
            while (dr.Read())
            {
                Session[dr["SessionKey"].ToString()] = dr["SessionValue"].ToString();
            }

            //Clean up database objects
            dr.Close();
            con.Close();
            cmd.Dispose();
            con.Dispose();
        }

		//This method removes all session information from the database identified by the 
		//  the GUID passed in through the parameter guidIn.
		private void ClearSessionFromDatabase(string guidIn)
		{
            /*
            try
            {
                //new RecordSet(DbConnection.ConnectionType_DB2).ExcecuteNonQuery("DELETE tblASPSessionState WHERE GUID = '" + guidIn + "'");
                //Get a DataReader that contains all the Session information
                var con = new DB2Connection(_connString);
                con.Open();
                var cmd = new DB2Command("DELETE tblASPSessionState WHERE GUID = '" + guidIn + "'", con);

                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                int i = 0;
            }
             */
		}

        private void ClearSessionFromDatabaseOld(string guidIn)
        {
            //**************************************
            //Enter connection information here

            SqlConnection con = new SqlConnection(_connString);

            //**************************************
            SqlCommand cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            string strSql;

            strSql = "DELETE FROM tblASPSessionState WHERE GUID = '" + guidIn + "'";
            cmd.CommandText = strSql;
            cmd.ExecuteNonQuery();

            con.Close();
            cmd.Dispose();
            con.Dispose();
        }

        /*
        public DB2DataReader GetDB2DataReader(string psqlCommand)
        {
            DB2DataReader _Results;
            var con = new DB2Connection(_connString);
            con.Open();
            var cmd = new DB2Command(psqlCommand, con);
            _Results = cmd.ExecuteReader();
            return _Results;
        }
        */

		//This method returns a new GUID as a string.
		private string GetGuid()
		{
			return System.Guid.NewGuid().ToString();
		}

        public static string GetAspLink(HttpRequest req, string aspFile, string parameters) {
        
            string newParams;
            if (parameters.Equals("")) {
	            newParams = "";
            }
            else {
		       newParams = "?" +parameters;
            }
            return "../../intranetx/session_transfer/SessionTransfer.aspx?dir=2asp&url=" + GlobalUtilArea.EncodeURL(CurServerURL(req) + "/v2/intranet/" + aspFile + newParams);
        }

        private static string CurServerURL(HttpRequest req)
        {
	        string s, protocol, port;
	        if (req.ServerVariables["HTTPS"].ToLower() == "on" ) 
		        s = "s";
	        else 
    		    s = "";

            protocol = StrLeft(req.ServerVariables["SERVER_PROTOCOL"], "/") + s;
	
	        if (req.ServerVariables["SERVER_PORT"] == "80")
		        port = "";
	        else
		        port = ":" + req.ServerVariables["SERVER_PORT"];
	        
	        return protocol + "://" + req.ServerVariables["SERVER_NAME"];
        }
    
        private static string CurPageURL(HttpRequest req)
        {
            string s, protocol, port;
	        if (req.ServerVariables["HTTPS"].ToLower() == "on" ) 
		        s = "s";
	        else 
    		    s = "";
            
	        protocol = StrLeft(req.ServerVariables["SERVER_PROTOCOL"], "/") + s;
	
	        if (req.ServerVariables["SERVER_PORT"] == "80")
		        port = "";
	        else
		        port = ":" + req.ServerVariables["SERVER_PORT"];
	        
	        return protocol + "://" + req.ServerVariables["SERVER_NAME"] + req.ServerVariables["SCRIPT_NAME"];
        }

        public static string  StrLeft(string str1, string str2) 
        {
            return str1.Substring(0, str1.IndexOf(str2));
        }
        
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
