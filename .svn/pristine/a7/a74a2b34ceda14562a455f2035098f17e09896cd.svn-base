using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.permissions
{
    public class PermissionsNewAction
    {
        public bool GrantChangeBeleg { get; set; }

        public PermissionsNewAction()
        {
            GrantChangeBeleg = false;
        }
       
        public void LoadPermissions(int userId)
        {
            GrantChangeBeleg = GlobalUtilArea.Rolecheck(411, userId);
        }
    }
}