namespace HTB.v2.intranetx.progress
{
    public partial class SyncTaskCallback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                // Button1
                string script1 = ScriptHelpers.GetStarterScript(this, "StartTask", "Button1");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "StartTask", script1, true);
                Button1.Attributes["onclick"] = "javascript:StartTask()";
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