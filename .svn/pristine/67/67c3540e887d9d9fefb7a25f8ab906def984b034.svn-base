using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.permissions
{
    public class PermissionsUEList
    {
        public bool GrantUEList { get; set; }
        public bool GrantUEListManager { get; set; }

        public PermissionsUEList()
        {
            GrantUEList = false;
            GrantUEListManager = false;
        }

        public void LoadPermissions(int userId)
        {
            GrantUEList = GlobalUtilArea.Rolecheck(57, userId);
            GrantUEListManager = GlobalUtilArea.Rolecheck(221, userId);
        }
    }
}