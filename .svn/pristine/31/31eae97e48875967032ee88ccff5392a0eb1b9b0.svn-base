using System.Web;

namespace HTB.v2.intranetx.progress.atlas
{
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

}