using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTBUtilities;

namespace HTB.v2.intranetx.global_files
{
    public partial class intranet_footer : System.Web.UI.UserControl
    {
        public static tblServerSettings settings = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (settings == null) 
                settings = (tblServerSettings)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblServerSettings", typeof(tblServerSettings));
        }
    }
}