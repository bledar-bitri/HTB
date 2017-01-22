using System.Web.UI;

namespace HTB.v2.intranetx.progress
{
    public partial class ContextSens : System.Web.UI.Page, ICallbackEventHandler
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                // Button1
                string script1 = ScriptHelpers.GetStarterScript(this, "StartTask", "Button1");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "StartTask", script1, true);
                Button1.Attributes["onclick"] = "javascript:StartTask()";

                // Button2
                string script2 = ScriptHelpers.GetStarterScript(this, "StartAnotherTask", "Button2");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "StartAnotherTask", script2, true);
                Button2.Attributes["onclick"] = "javascript:StartAnotherTask()";
            }
        }

        #region "ICallbackEventHandler"
        private string _taskID;
        public void RaiseCallbackEvent(string argument)
        {
            _taskID = argument;
        }

        public string GetCallbackResult()
        {
            LengthyTask task = new LengthyTask(_taskID);

            // We don't support arguments--to do that, serialize/deserialize "id:your data"
            task.Run();
            return task.Results.ToString();
        }
        #endregion

    }
}