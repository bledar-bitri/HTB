using System;
using System.Collections.Specialized;
using System.Web.UI;

namespace HTB.v2.intranetx.paypal
{
    public partial class PaypalPaymentResponse : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (string param in Request.Params )
            {
                lblMessage.Text += param+"<br/>";
            }
        }
    }
}