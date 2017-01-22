using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.permissions
{
    public class PermissionsRoutePlanner
    {
        public bool GrantRoutePlannerForAll { get; set; }
        public bool GrantRoutePlannerForSelfOnly { get; set; }
        public PermissionsRoutePlanner()
        {
            GrantRoutePlannerForAll = false;
            GrantRoutePlannerForSelfOnly = false;
        }

        public void LoadPermissions(int userId)
        {
            GrantRoutePlannerForAll = GlobalUtilArea.Rolecheck(413, userId);
            GrantRoutePlannerForSelfOnly = GlobalUtilArea.Rolecheck(412, userId);
        }
    }
}