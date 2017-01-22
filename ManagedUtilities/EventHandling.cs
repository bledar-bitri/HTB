using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ManagedUtilities
{
  public  class EventHandling
    {
        public void WriteEvent(string SourceName, string LogName, string Error, EventLogEntryType evtType)
        {
            if (!EventLog.SourceExists(SourceName))
                EventLog.CreateEventSource(SourceName, LogName);

            EventLog.WriteEntry(SourceName, Error, evtType);
        }
    }
}
