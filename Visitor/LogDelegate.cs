using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VisitorDelegate
{
    /// <summary>
    /// Методов Match может быть несколько, принимающих разный набор делегатов
    /// </summary>
    public abstract class LogEntry
    {
        public void Match(Action<ExceptionLogEntry> exceptionEntryMatch, Action<SimpleLogEntry> simpleEntryMatch)
        {
            var exceptionLogEntry = this as ExceptionLogEntry;
            if (exceptionLogEntry != null)
            {
                exceptionEntryMatch(exceptionLogEntry);
                return;
            }
            var simpleLogEntry = this as SimpleLogEntry;
            if (simpleLogEntry != null)
            {
                simpleEntryMatch(simpleLogEntry);
                return;
            }
            throw new InvalidOperationException("Unknow LogEntry type");
        }
    }

    public class SimpleLogEntry: LogEntry
    {
    }

    public class ExceptionLogEntry: LogEntry
    {
    }

    public class DataBaseLogSaver
    {
        public void SaveLogEntry(LogEntry logEntry)
        {
            logEntry.Match(ex => SaveException(ex), simple => SaveSimpleLogEntry(simple));
        }

        private void SaveSimpleLogEntry(object simle)
        {
            throw new NotImplementedException();
        }

        private void SaveException(ExceptionLogEntry ex)
        {
            throw new NotImplementedException();
        }
    }
}
