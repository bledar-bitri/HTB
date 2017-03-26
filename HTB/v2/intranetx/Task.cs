using System;
using System.Threading;
using System.Web;

namespace HTB.v2.intranetx
{
    public abstract class Task
    {
        protected object _results;

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

    public class LengthyTask : Task
    {

        private int _seconds;
        public LengthyTask(string taskID)
            : base(taskID)
        {
            _seconds = 5;
        }

        public int Seconds
        {
            get { return _seconds; }
            set { _seconds = value; }
        }

        public override void Run()
        {
            for (int i = 1; i <= _seconds; i++)
            {
                TaskHelpers.UpdateStatus(_taskID, string.Format("Step {0} out of {1}", i, _seconds));
                Thread.Sleep(1000);
            }

            Thread.Sleep(1000);
            _results = string.Format("Task completed at {0}", DateTime.Now);
            TaskHelpers.ClearStatus(_taskID);
        }

        public override void Run(object data)
        {
            for (int i = 1; i <= _seconds; i++)
            {
                TaskHelpers.UpdateStatus(_taskID, string.Format("Step {0} out of {1}", i, _seconds));
                Thread.Sleep(1000);
            }

            Thread.Sleep(1000);
            _results = string.Format("Task completed at {0} : Data=‘{1}’", DateTime.Now, data);
            TaskHelpers.ClearStatus(_taskID);
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

            object o = context.Cache[taskID];

            if (o == null)
                return string.Empty;

            return (string)o;
        }

        public static void ClearStatus(string taskID)
        {
            HttpContext context = HttpContext.Current;
            context.Cache.Remove(taskID);
        }
    }

}