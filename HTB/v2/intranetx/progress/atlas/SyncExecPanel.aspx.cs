using System;

namespace HTB.v2.intranetx.progress.atlas
{
    
    partial class SyncExecPanel : System.Web.UI.Page
    {

        protected void Button1_Click(object sender, System.EventArgs e)
        {
            TimeServed.Text = "Served at " + DateTime.Now.ToString("hh:mm:ss");
        }

        // Use this code if you want to programmatically set (once for all) the wait text 
        //Protected Sub progress1_OnLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles progress1.Load
        //    DirectCast(progress1.FindControl("Msg"), Label).Text = "It may take a while..."
        //End Sub
    }

}