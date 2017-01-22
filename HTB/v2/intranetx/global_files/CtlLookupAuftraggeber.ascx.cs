using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlLookupAuftraggeber : System.Web.UI.UserControl
    {
        public string ComponentName = "";
        public Page parent;

        public void SetParent(Page parent)
        {
            this.parent = parent;
        }
        public void SetComponentName(string name)
        {
            ComponentName = name;
        }

        public void SetNextFocusableComponentId(string componentId)
        {
            hdnNextFocusId.Value = componentId;
        }
   
        public string GetAuftraggeberID()
        {
            return hdnAuftraggeberId.Value;
        }
        
        public void SetAuftraggeber(tblAuftraggeber ag)
        {
            txtAuftraggeberSearch.Text = ag.AuftraggeberName1 + " " + ag.AuftraggeberName2;
            lblAuftraggeberAddress.Text = "<strong>Addresse</strong>: " + ag.AuftraggeberStrasse + ", " + ag.AuftraggeberPLZ + ", " + ag.AuftraggeberOrt;
            hdnAuftraggeberId.Value = ag.AuftraggeberID.ToString();
        }
        public TextBox GetTextBox()
        {
            return txtAuftraggeberSearch;
        }

        public void Clear()
        {
            txtAuftraggeberSearch.Text = "";
            lblAuftraggeberAddress.Text = "";
            hdnAuftraggeberId.Value = "";
        }
    }
}