using System;
using HTB.TabletService;

namespace HTB.v2.intranetx
{
    public partial class TestProgress : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var client = new GetDataClient("BasicHttpBinding_IGetData");
            lblMessage.Text = client.GetPhoneTypes();
            lblMessage.Text += "<br/>";
            lblMessage.Text += client.GetAktTypes();

        }
    }
}