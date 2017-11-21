using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMethod
{
    class LogSaverWCF
    {
    }

    interface ILogSaver
    {
        void UploadLogEntries(IEnumerable<LogEntry> logEntries);
        void UploadException(IEnumerable<ExceptionLogEntry> exception);
    }

    class LogSaverProxy : ILogSaver
    {
        class LogSaverClient : ClientBase<ILogSaver>
        {
            public ILogSaver LogSaver
            {
                get { return Channel; }
            }
        }

        public void UploadException(IEnumerable<ExceptionLogEntry> exception)
        {
            UseProxyClient(c => c.UploadException(exception));
        }

        public void UploadLogEntries(IEnumerable<LogEntry> logEntries)
        {
            UseProxyClient(c => c.UploadLogEntries(logEntries));
        }
        // Шаблонный метод с использованием делегатов
        public void UseProxyClient(Action<ILogSaver> accessor)
        {
            var client = new LogSaverClient();
            try
            {
                accessor(client.LogSaver);
                client.Close();
            }
            catch (CommunicationException e)
            {
                client.Abort();
                throw new OperationFailedException(e);

            }
        }
    }
}
