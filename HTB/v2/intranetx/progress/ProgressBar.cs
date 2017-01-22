using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HTB.v2.intranetx.progress
{
   
    public class ProgressPanel : CompositeControl, ICallbackEventHandler
    {


        private Label _label;
        #region "Properties"
        public int RefreshRate
        {
            get
            {
                object o = ViewState["RefreshRate"];
                if (o == null)
                {
                    return 1;
                }
                return (int)o;
            }
            set { ViewState["RefreshRate"] = value; }
        }
        #endregion

        #region "Composition"
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Clear();
            CreateControlHierarchy();
            ClearChildViewState();
        }

        protected virtual void CreateControlHierarchy()
        {
            _label = new Label();
            _label.ID = "Status";

            // Save the control tree
            Controls.Add(_label);

            string js = GetRefreshScript();
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "ShowProgress", js, true);

        }

        protected override void Render(HtmlTextWriter writer)
        {
            // Avoid a surrounding <span> tag
            base.RenderContents(writer);
        }

        #endregion

        #region "Script"

        private string GetRefreshScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine("var timerID;");

            sb.AppendLine("function ShowProgress() {");
            sb.AppendLine(Page.ClientScript.GetCallbackEventReference(this, "taskID", "UpdateStatus", "null", true));
            sb.AppendFormat("  timerID = window.setTimeout(\"ShowProgress()\", {0});", RefreshRate * 1000);
            sb.AppendLine("}");
            sb.AppendLine(GetUpdateStatus());
            sb.AppendLine("function StopProgress() {");
            sb.AppendFormat("  var label = document.getElementById('{0}_Status');", ID);
            sb.AppendLine();
            sb.AppendLine("  label.innerHTML = '';");
            sb.AppendLine("  window.clearTimeout(timerID);");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private string GetUpdateStatus()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("function UpdateStatus(response, context) {");
            sb.AppendFormat("  var label = document.getElementById('{0}_Status');", ID);
            sb.AppendLine();
            sb.AppendLine("  label.innerHTML = response;");
            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion

        #region "ICallbackEventHandler"
        private string _taskID;
        public void RaiseCallbackEvent(string argument)
        {
            _taskID = argument;
        }

        public string GetCallbackResult()
        {
            return TaskHelpers.GetStatus(_taskID);
        }

        #endregion

    }

}