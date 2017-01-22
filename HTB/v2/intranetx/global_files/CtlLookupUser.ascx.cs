using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.Database.Views;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlLookupUser : System.Web.UI.UserControl
    {
        public string ComponentName = "";

        public void SetComponentName(string name)
        {
            ComponentName = name;
        }

        public void SetNextFocusableComponentId(string componentId)
        {
            hdnNextFocusId.Value = componentId;
        }

        public string GetUserID()
        {
            return hdnUserId.Value;
        }

        public void SetUser(qryUsers user)
        {
            hdnUserId.Value = user.UserID.ToString();
            txtUserSearch.Text = user.UserVorname + " " + user.UserNachname;
            lblUserDescription.Text = "<strong>Abteilung</strong>: " + user.AbteilungCaption;
        }

        public TextBox GetTextBox()
        {
            return txtUserSearch;
        }

        public void Clear()
        {
            txtUserSearch.Text = "";
            lblUserDescription.Text = "";
            hdnUserId.Value = "";
        }
    }
}