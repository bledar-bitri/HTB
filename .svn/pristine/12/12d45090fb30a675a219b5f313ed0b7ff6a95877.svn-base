using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.permissions
{
    public class PermissionsADStatistic
    {
        public bool GrantReports { get; set; }
        
        public PermissionsADStatistic()
        {
            GrantReports = false;
        }

        
        public void LoadPermissions(int userId)
        {
            GrantReports = GlobalUtilArea.Rolecheck(409, userId);
        }
    }
}