using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HTB.intranetx.aktenint
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetExpires(DateTime.Parse(DateTime.Now.ToString()));
            Response.Cache.SetCacheability(HttpCacheability.Private);
            int wuserId = -1;
            //Session["MM_UserID"] = 99;
            try
            {
                wuserId = Convert.ToInt32(Session["MM_UserID"]);
            }
            catch { }
            msg.Text = "ASDF";
        }
    }
}