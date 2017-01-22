using System.Text;
using System.Web;
using System.Web.UI;

namespace HTB.v2.intranetx.progress
{
    public abstract class Task
    {
        protected object _results;
        protected string _status;

        protected string _taskID;
        public Task(string taskID)
        {
            _taskID = taskID;
        }

        public abstract void Run();
        public abstract void Run(object data);

        public object Results
        {
            get { return _results; }
        }
    }

    public class TaskHelpers
    {
        public static void UpdateStatus(string taskID, string info)
        {
            HttpContext context = HttpContext.Current;
            context.Cache[taskID] = info;
        }

        public static string GetStatus(string taskID)
        {
            HttpContext context = HttpContext.Current;

            object o = null;
            o = context.Cache[taskID];

            if (o == null)
            {
                return string.Empty;
            }

            return (string)o;
        }

        public static void ClearStatus(string taskID)
        {
            HttpContext context = HttpContext.Current;
            context.Cache.Remove(taskID);
        }
    }

    public static class ScriptHelpers
    {
        public static string GetStarterScript(Page pageObject, string funcName, string ctlName)
        {
            HttpContext context = HttpContext.Current;

            var sb = new StringBuilder();
            sb.AppendLine("function GetGuid() {");
            sb.AppendLine("   var ranNum = Math.floor(Math.random()*100000000);");
            sb.AppendLine("   return ranNum;");
            sb.AppendLine("}");

            sb.AppendLine("var taskID;");

            sb.AppendFormat("function {0}()", funcName);
            sb.AppendLine(" {");
            sb.AppendFormat("  var ctl = document.getElementById('{0}');", ctlName);
            sb.AppendLine("");
            sb.AppendLine("  ctl.disabled = true;");
            sb.AppendLine("  var id = GetGuid();");
            sb.AppendLine("  taskID = id; ");
            sb.AppendLine("  StartProgress();");
//            sb.AppendLine("  ShowProgress();");
            sb.AppendLine(pageObject.ClientScript.GetCallbackEventReference(pageObject, "id", "StartProgress", "null", false));
            sb.AppendLine("}");
            return sb.ToString();
        }
    }

}