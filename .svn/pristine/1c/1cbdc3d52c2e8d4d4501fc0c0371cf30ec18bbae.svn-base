using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using HTB.v2.intranetx.util;
using HTBUtilities;
using HTB.Database;
using System.Data;
using HTB.Database.LookupRecords;
using System.ComponentModel;
using System.Text;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlBrowserUser : System.Web.UI.UserControl
    {
        private static int usersCount;
        private static string userName = "";
        private static string userDept = GlobalUtilArea.DefaultDropdownValue;
        private static int startIndex = 0;
        private static int endIndex = 0;
        private static string returnToURL = "";
        private static string returnExtraParams = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GlobalUtilArea.LoadDropdownList(ddlUserDepartment,
                    "SELECT * FROM tblAbteilung ORDER BY AbteilungCaption ASC",
                    typeof(tblAbteilung),
                    "AbteilungID",
                    "AbteilungCaption",
                    true
                    );
                txtUserName.Text = "";
                userName = "";
                userDept = GlobalUtilArea.DefaultDropdownValue;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            userName = txtUserName.Text;
            userDept = ddlUserDepartment.SelectedValue;
            gvUsers.DataBind();
            lblTotalUsersMsg.Text = "Anwender " + CtlBrowserUser.startIndex + " bis " + CtlBrowserUser.endIndex + " von " + CtlBrowserUser.usersCount;
            txtUserName.Focus();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(GetReturnToUrl(returnToURL, GlobalUtilArea.DecodeUrl(returnExtraParams), null));
        }

        public void gvUsers_DataBound(object sender, EventArgs e)
        {
            lblTotalUsersMsg.Text = "Anwender " + CtlBrowserUser.startIndex + " bis " + CtlBrowserUser.endIndex + " von " + CtlBrowserUser.usersCount;
        }

        public List<UserLookup> GetUsers(int startIndex, int pageSize, string sortBy, ref int totalUsers)
        {
            ArrayList parameters = new ArrayList();
            parameters.Add(new StoredProcedureParameter("@startIndex", SqlDbType.Int, startIndex));
            parameters.Add(new StoredProcedureParameter("@pageSize", SqlDbType.Int, pageSize));
            parameters.Add(new StoredProcedureParameter("@sortBy", SqlDbType.Int,sortBy));
            parameters.Add(new StoredProcedureParameter("@totalUsers", SqlDbType.Int,totalUsers, ParameterDirection.Output));
            
            if (userName != null && userName.Trim() != string.Empty)
                parameters.Add(new StoredProcedureParameter("@name", SqlDbType.NVarChar, userName));
            if (userDept != GlobalUtilArea.DefaultDropdownValue)
                parameters.Add(new StoredProcedureParameter("@department", SqlDbType.Int, userDept));

            ArrayList list = HTBUtils.GetStoredProcedureRecords("spSearchUsers", parameters, typeof(UserLookup));
            foreach (UserLookup user in list)
            {
                user.UserNameLink = GetReturnToUrl(returnToURL, returnExtraParams, user);
                if (user.UserStatus == 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<font color=#999999>");
                    sb.Append(user.UserDepartment);
                    sb.Append(" (Status Inaktiv)");
                    sb.Append("</font>");
                    user.UserDepartment = sb.ToString();

                    sb.Clear();
                    sb.Append("<font color=#999999>");
                    sb.Append(user.UserID);
                    sb.Append("</font>");
                    user.UserID = sb.ToString();
                }
            }
            foreach (object o in parameters)
            {
                if (o is ArrayList)
                {
                    ArrayList outputList = (ArrayList)o;
                    foreach (StoredProcedureParameter p in outputList)
                    {
                        if (p.Name.IndexOf("totalUsers") >= 0)
                        {
                            totalUsers = Convert.ToInt32(p.Value);
                        }
                    }
                }
            }
            CtlBrowserUser.startIndex = startIndex+1;
            CtlBrowserUser.endIndex = startIndex+list.Count;
            return list.Cast<UserLookup>().ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<UserLookup> GetAllUsers(int startIndex, int pageSize, string sortBy)
        {

            List<UserLookup> result;
            int totalUsers=0;
            if (string.IsNullOrEmpty(sortBy))
                sortBy = "UserStatus DESC, UserVorname";
            result = GetUsers(startIndex, pageSize, sortBy, ref totalUsers);
            usersCount = totalUsers;
            return result;
        }

        private string GetReturnToUrl(string returnToUrlCode, string returnExtraParams, UserLookup client)
        {
            switch (returnToUrlCode)
            {
                case GlobalHtmlParams.URL_NEW_AKT:
                    StringBuilder sb = new StringBuilder();
                    sb.Append("../aktenink/NewAktInk.aspx?");
                    if (client != null)
                    {
                        sb.Append(GlobalHtmlParams.USER_ID);
                        sb.Append("=");
                        sb.Append(client.UserID.ToString());

                    }
                    if (returnExtraParams != "")
                    {
                        if (client != null)
                            sb.Append("&");
                        sb.Append(HttpUtility.UrlPathEncode(returnExtraParams));
                    }

                    return sb.ToString();
            }
            return "";
        }

        public int GetTotalUsersCount()
        {
            return usersCount;
        }

        public void SetReturnToURL(string ret)
        {
            returnToURL = ret;
        }
        public void SetReturnExtraParams(string ret)
        {
            returnExtraParams = ret;
        }
    }
}