using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.permissions
{
    public class PermissionsProvision
    {
        public bool GrantFixprov { get; set; }
        public bool GrantFixprovNew { get; set; }
        public bool GrantFixprovEdit { get; set; }
        public bool GrantFixprovDelete { get; set; }
        public bool GrantFixprovPrint { get; set; }

        public PermissionsProvision()
        {
            GrantFixprovPrint = false;
            GrantFixprovDelete = false;
            GrantFixprovEdit = false;
            GrantFixprovNew = false;
            GrantFixprov = false;
        }

        
        public void LoadPermissions(int userId)
        {
            GrantFixprov = GlobalUtilArea.Rolecheck(121, userId);
            GrantFixprovNew = GlobalUtilArea.Rolecheck(122, userId);
            GrantFixprovEdit = GlobalUtilArea.Rolecheck(123, userId);
            GrantFixprovDelete = GlobalUtilArea.Rolecheck(124, userId);
            GrantFixprovPrint = GlobalUtilArea.Rolecheck(240, userId);
        }
    }
}