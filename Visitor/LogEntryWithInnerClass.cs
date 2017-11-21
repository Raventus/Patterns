using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorInnet
{
    public class LogEntryVisitorBase : ILogEntryVisitor
    {
        public virtual void Visit(ExceptionLogEntry exceptionLogEntry)
        { }
        public virtual void Visit(SimpleLogEntry simpleLogEntry)
        { }
    }

    public class DataBaseExceptionLogEntrySaver : LogSaverBase
    {
        public void SaveLogEntry(LogEntry logEntry)
        {
            logEntry.Accept(new ExceptionLogEntryVisitor(this));
        }
        private class ExceptionLogEntryVisitor : LogEntryVisitorBase
        {
            private readonly DataBaseExceptionLogEntrySaver _parrent;
            public ExceptionLogEntryVisitor(DataBaseExceptionLogEntrySaver parrent)
            {
                _parrent = parrent;
            }
            public override void Visit(ExceptionLogEntryVisitor expectionLogEntry)
            {
                _parrent.SaveException(exceptionLogEntry);
            }
        }
    }

public class LogSaverBase
    {
    }

    public interface ILogEntryVisitor
    {
    }
}
