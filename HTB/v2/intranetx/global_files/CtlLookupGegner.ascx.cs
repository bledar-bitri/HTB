using System.Web.UI.WebControls;
using HTB.Database;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlLookupGegner : System.Web.UI.UserControl
    {
        public string ComponentName = "";
        public string Category { get; set; }

        public void SetComponentName(string name)
        {
            ComponentName = name;
            contactExtender.OnClientItemSelected = GetJavaScriptSelectMethodName();
        }

        public void SetNextFocusableComponentId(string componentId)
        {
            hdnNextFocusId.Value = componentId;
        }

        public string GetGegnerID()
        {
            return hdnGegnerId.Value;
        }

        public string GetGegnerOldID()
        {
            return hdnGegnerOldId.Value;
        }

        public void SetGegner(tblGegner gegner)
        {
            hdnGegnerId.Value = gegner.GegnerID.ToString();
            hdnGegnerOldId.Value = gegner.GegnerOldID;
            txtGegnerSearch.Text = gegner.GegnerLastName1 + " " + gegner.GegnerLastName2;
            lblGegnerAddress.Text = "<strong>Addresse</strong>: " + gegner.GegnerLastStrasse + ", " + gegner.GegnerLastZip + ", " + gegner.GegnerLastOrt;
        }

        public TextBox GetTextBox()
        {
            return txtGegnerSearch;
        }

        public void Clear()
        {
            txtGegnerSearch.Text = "";
            lblGegnerAddress.Text = "";
            hdnGegnerId.Value = "";
            hdnGegnerOldId.Value = "";
        }

        public string GetJavaScriptSelectMethodName()
        {
            return ComponentName + "_OnGegnerSelected";
        }
    }
}