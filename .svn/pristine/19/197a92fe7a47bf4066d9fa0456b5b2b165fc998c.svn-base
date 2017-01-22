using System;
using System.Web;
using System.Data.SqlClient;
using HTB.v2.intranetx.util;

namespace HTB.session_transfer
{
	/// <summary>
	/// Summary description for SessionTransfer.
	/// </summary>
	public partial class SessionTransfer : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
            Response.Cache.SetExpires(DateTime.Parse(DateTime. Now.ToString()));
            Response.Cache.SetCacheability(HttpCacheability.Private);
			string guidSave;
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
			    ClearSessionFromDatabase(Request.QueryString["guid"]);
				Response.Redirect(GlobalUtilArea.DecodeUrl(Request.QueryString["url"]));

                //Response.Write("GUID: "+Request.QueryString["guid"] + "<br>");
                //Response.Write("URL:  "+ Request.QueryString["url"] + "<br>");
			}
		}


		//This method adds the session information to the database and returns the GUID
		//  used to identify the data.
		private string AddSessionToDatabase()
		{

			//**************************************
			//Enter connection information here

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["HTBConnectionString"]);
			
			//**************************************

			SqlCommand cmd = new SqlCommand();
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
			//**************************************
			//Enter connection information here

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["HTBConnectionString"]);
			
			//**************************************
			SqlCommand cmd = new SqlCommand();
			SqlDataReader dr;
			con.Open();
			cmd.Connection = con;

			string strSql, guidTemp = GetGuid();
	
			//Get a DataReader that contains all the Session information
			strSql = "SELECT * FROM tblASPSessionState WHERE GUID = '" + guidIn + "'";
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
			//**************************************
			//Enter connection information here

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["HTBConnectionString"]);
			
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
