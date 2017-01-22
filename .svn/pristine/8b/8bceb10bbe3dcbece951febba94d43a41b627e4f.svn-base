using System.Threading;
using System;

namespace HTB.v2.intranetx.progress
{
    public class LengthyTask : Task
    {

        private int _seconds;
        public LengthyTask(string taskID) : base(taskID)
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
            _results = string.Format("Task completed at {0} : Data='{1}'", DateTime.Now, data);
            TaskHelpers.ClearStatus(_taskID);
        }

    }

}