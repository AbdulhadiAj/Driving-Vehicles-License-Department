using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DVLD_DataAccessLayer
{
    public static class clsEventLogger
    {

        public static string SourceName = "DVLD-Application";

        public static void LogError(string message)
        {
            if (!EventLog.SourceExists(SourceName))
                EventLog.CreateEventSource(SourceName, "Application");

            EventLog.WriteEntry(SourceName, message, EventLogEntryType.Error);
        }

        public static void LogInformation(string message)
        {
            if (!EventLog.SourceExists(SourceName))
                EventLog.CreateEventSource(SourceName, "Application");

            EventLog.WriteEntry(SourceName, message, EventLogEntryType.Information);
        }

        public static void LogWarning(string message)
        {
            if (!EventLog.SourceExists(SourceName))
                EventLog.CreateEventSource(SourceName, "Application");

            EventLog.WriteEntry(SourceName, message, EventLogEntryType.Warning);
        }

    }
}
