using System;
using System.Runtime.Serialization;

namespace HTB.v2.intranetx.progress
{
    [Serializable]
    public class TaskStatus : ISerializable
    {
        public int Progress;
        public string ProgressText;
        public string ProgressTextLong;

        public TaskStatus()
        {
        }

        #region ISerializable
        //note: this is private to control access;
        //the serializer can still access this constructor
        private TaskStatus(SerializationInfo info, StreamingContext context)
        {
            Progress = info.GetInt32("Progress");
            ProgressText = info.GetString("ProgressText");
            ProgressTextLong = info.GetString("ProgressTextLong");
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Progress", Progress);
            info.AddValue("ProgressText", ProgressText);
            info.AddValue("ProgressTextLong", ProgressTextLong);
        }
        #endregion
    }
}