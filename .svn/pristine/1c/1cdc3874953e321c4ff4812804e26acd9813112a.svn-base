using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.permissions
{
    public class PermissionsEditAktInk
    {
        public bool GrantInkassoEdit { get; set; }
        public bool GrantInkassoNew { get; set; }
        public bool GrantInkassoForderungNew { get; set; }
        public bool GrantInkassoForderungEdit { get; set; }
        public bool GrantInkassoForderungDel { get; set; }
        public bool GrantInkassoDokumentNew { get; set; }
        public bool GrantInkassoDokumentDel { get; set; }
        public bool GrantInkassoBuchungNew { get; set; }
        public bool GrantInkassoBuchungEdit { get; set; }
        public bool GrantInkassoBuchungDel { get; set; }
        public bool GrantInkassoAktAktionNew { get; set; }
        public bool GrantInkassoAktAktionEdit { get; set; }
        public bool GrantInkassoAktAktionDel { get; set; }
        public bool GrantAktINfo { get; set; }

        public PermissionsEditAktInk()
        {
            GrantAktINfo = false;
            GrantInkassoAktAktionDel = false;
            GrantInkassoAktAktionEdit = false;
            GrantInkassoAktAktionNew = false;
            GrantInkassoBuchungDel = false;
            GrantInkassoBuchungEdit = false;
            GrantInkassoBuchungNew = false;
            GrantInkassoDokumentDel = false;
            GrantInkassoDokumentNew = false;
            GrantInkassoForderungDel = false;
            GrantInkassoForderungEdit = false;
            GrantInkassoForderungNew = false;
            GrantInkassoNew = false;
            GrantInkassoEdit = false;
        }

        
        public void LoadPermissions(int userId)
        {
            GrantInkassoEdit = GlobalUtilArea.Rolecheck(65, userId);
            GrantInkassoNew = GlobalUtilArea.Rolecheck(253, userId);
            GrantInkassoForderungNew = GlobalUtilArea.Rolecheck(352, userId);
            GrantInkassoForderungEdit = GlobalUtilArea.Rolecheck(350, userId);
            GrantInkassoForderungDel = GlobalUtilArea.Rolecheck(351, userId);
            GrantInkassoDokumentNew = GlobalUtilArea.Rolecheck(354, userId);
            GrantInkassoDokumentDel = GlobalUtilArea.Rolecheck(355, userId);
            GrantInkassoBuchungNew = GlobalUtilArea.Rolecheck(357, userId);
            GrantInkassoBuchungEdit = GlobalUtilArea.Rolecheck(358, userId);
            GrantInkassoBuchungDel = GlobalUtilArea.Rolecheck(359, userId);
            GrantInkassoAktAktionNew = GlobalUtilArea.Rolecheck(360, userId);
            GrantInkassoAktAktionEdit = GlobalUtilArea.Rolecheck(361, userId);
            GrantInkassoAktAktionDel = GlobalUtilArea.Rolecheck(362, userId);
            GrantAktINfo = GlobalUtilArea.Rolecheck(380, userId);
        }
    }
}