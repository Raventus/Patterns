using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor
{
    public abstract class LogEntry
    {
        public abstract void Accept(ILogEntryVisitor logEntryVisitor);
    }

    public interface ILogEntryVisitor
    {
        void Visit(LogEntry exceptionLogEntry);
    }

    public class DataBaseLogSever : ILogEntryVisitor
    {
        public void Visit(ExceptionLogEntry exceptionLogEntry)
        {
            // Сохранение класса с логами экспептион
        }
        public void Visit(SimpleLogEntry exceptionLogEntry)
        {
            // Сохранение класса с простыми логами
        }
        public void SaveLogEntry(LogEntry logEntry)
        {
            logEntry.Accept(this);
        }
    }

    public class ExceptionLogEntry : LogEntry
    {
        public override void Accept(ILogEntryVisitor logEntryVisitor)
        {
            // Благодаря перегрузки методов выбирается метод Visit(ExceptionLogEntry)
            logEntryVisitor.Visit(this);
        }
    }

     public class SimpleLogEntry : LogEntry
    {
        public override void Accept(ILogEntryVisitor logEntryVisitor)
        {
            // Благодаря перегрузки методов выбирается метод Visit(ExceptionLogEntry)
            logEntryVisitor.Visit(this);
        }
    }
}
