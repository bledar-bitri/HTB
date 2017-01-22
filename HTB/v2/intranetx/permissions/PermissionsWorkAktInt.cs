using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.permissions
{
    public class PermissionsWorkAktInt
    {
        public bool GrantInkasso { get; set; }

        public PermissionsWorkAktInt()
        {
            GrantInkasso = false;
        }

        public void LoadPermissions(int userId)
        {
            GrantInkasso = GlobalUtilArea.Rolecheck(280, userId);
        }
    }
}