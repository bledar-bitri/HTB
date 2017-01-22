using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlLookupDealer : UserControl
    {
        public string ComponentName = "";
        
        public void SetComponentName(string name)
        {
            ComponentName = name;
        }

        public void SetNextFocusableComponentId(string componentId)
        {
            hdnNextFocusId.Value = componentId;
        }
   
        public string GetDealerID()
        {
            return hdnDealerId.Value;
        }

        
        public void SetDealer(tblAutoDealer dealer)
        {
            txtDealerSearch.Text = dealer.AutoDealerName;
            lblDealerAddress.Text = "<strong>Addresse</strong>: " + dealer.AutoDealerStrasse + ", " + dealer.AutoDealerPLZ + ", " + dealer.AutoDealerOrt;
            hdnDealerId.Value = dealer.AutoDealerID.ToString();
        }
        public TextBox GetTextBox()
        {
            return txtDealerSearch;
        }

        public void Clear()
        {
            txtDealerSearch.Text = "";
            lblDealerAddress.Text = "";
            hdnDealerId.Value = "";
        }
    }
}