using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlLookupKlient : System.Web.UI.UserControl
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
   
        public string GetKlientID()
        {
            return hdnKlientId.Value;
        }

        public string GetKlientOldID()
        {
            return hdnKlientOldId.Value;
        }
        public string GetKlientAddress()
        {
            return lblKlientAddress.Text;
        }
        public void SetKlientAddress(string address)
        {
            lblKlientAddress.Text = address;
        }
        public void SetKlient(tblKlient klient)
        {
            txtKlientSearch.Text = klient.KlientName1 + " " + klient.KlientName2;
            lblKlientAddress.Text = "<strong>Addresse</strong>: " + klient.KlientStrasse + ", " + klient.KlientPLZ + ", " + klient.KlientOrt;
            hdnKlientId.Value = klient.KlientID.ToString();
            hdnKlientOldId.Value = klient.KlientOldID;
        }
        public TextBox GetTextBox()
        {
            return txtKlientSearch;
        }

        public void Clear()
        {
            txtKlientSearch.Text = "";
            lblKlientAddress.Text = "";
            hdnKlientId.Value = "";
            hdnKlientOldId.Value = "";
        }

    }
}