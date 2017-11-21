using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Observer
{
    /// <summary>
    /// Наблюдатель определяет зависимость типа 1 ко многим между объектами таким образом, что при изменении
    /// состояния одного объекта все зависящие от него оповещаются об этом и автоматически обновляются
    /// Наблюдатель уведомляет все заинетесованные стороны о произошедшем событии или об изменении своего состояния
    /// 1. Делегат (callback) 2. События 3. Спец. интерфейс наблюдателя 4. IObserver/IObservable
    /// Push- модель (Callback вызов) EventHadler<T>: where T: EventArgs пример pull/push модели. Событие происходит(push), и наблюдатель может взять нужную информацию через EventArgs (pull)
    /// Существует проблема утечки памяти: чтобы изюежать: избегаем долгоживущих объектов с событиями, наблюдатели используют интерфейс Idisposable и отписываются от событий в методе Dispose, использование слабых событий со слабыми ссылками
    /// Паттерн посредник очень часто реализован в виде наблюдателя ( связывая две независимые части приложения воедино)
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Subject subject = new Subject();
            Observer observer = new Observer(subject, "Center", "\t\t");
            Observer observer2 = new Observer(subject, "Right", "\t\t\t\t");
            subject.Go();
            Console.ReadKey();
        }


    }

    class Simulator : IEnumerable
    {
        string[] moves = { "5", "3", "1", "6", "7" };
        public IEnumerator GetEnumerator()
        {
            foreach (string element in moves)
                yield return element;
        }
    }

    interface ISubject
    {
        void AddObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers(string s);
    }
    class Subject : ISubject
    {
        public string SubjectState { get; set; }
        public List<IObserver> Observers { get; private set; }

        private Simulator simulator;

        private const int speed = 200;

        public Subject()
        {
            Observers = new List<IObserver>();
            simulator = new Simulator();
        }

        public void AddObserver(IObserver observer)
        {
            Observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            Observers.Remove(observer);
        }

        public void NotifyObservers(string s)
        {
            foreach (var observer in Observers)
            {
                observer.Update(s);
            }
        }

        public void Go()
        {
            new Thread(new ThreadStart(Run)).Start();
        }

        void Run()
        {
            foreach (string s in simulator)
            {
                Console.WriteLine("Subject: " + s);
                SubjectState = s;
                NotifyObservers(s);
                Thread.Sleep(speed); // milliseconds
            }
        }
    }

    interface IObserver
    {
        void Update(string state);
    }

    class Observer : IObserver
    {
        string name;

        ISubject subject;

        string state;

        string gap;

        public Observer(ISubject subject, string name, string gap)
        {
            this.subject = subject;
            this.name = name;
            this.gap = gap;
            subject.AddObserver(this);
        }

        public void Update(string subjectState)
        {
            state = subjectState;
            Console.WriteLine(gap + name + ": " + state);
        }
    }



    // 1. Делегат отношение 1 к 1
    // Используем: наблюдатель должен быть обязательно, наблюдаемый объект не просто уведомляет наблюдателя, но и ожидает некоторого результата
    // Не используем: Число делегатов начинает расти (лучше 3 способ) для повторно используемых компонентов (2 способ) через делегат передается поток событий (4 способ).
    public class LogFileReader : IDisposable
    {
        private readonly string _logFileReader;
        private readonly Action<string> _lofEntrySubscriber;
        private readonly TimeSpan CheckFileInterval = TimeSpan.FromSeconds(5);
        private readonly Timer _timer;

        // Класс требует делегат в качестве аргумента в конструктора
        public LogFileReader(string logFileName, Action<string> logEntrySubscriber)
        {
            _lofEntrySubscriber = logEntrySubscriber;
            _timer = new Timer(() => CheckFile(), CheckFileInterval, CheckFileInterval);
        }
        public void Dispose()
        {
            _timer.Dispose();
        }

        private void CheckFile()
        {
            foreach (var logEntry in ReadNewLogEntries())
            {
                _lofEntrySubscriber(logEntry);
            }
        }


        private IEnumerable<string> ReadNewLogEntries()
        {
            yield return "1";
        }
    }

    // 2 Событие 1 ко многим
    // Событие не гарантирует наличие подписчиков, а значит не может требовать результат
    // Используем: для повторно используемых компонентов, для уведомления множества наблюдателей, когда не ожидаешь от них каких-либо действий
    // для реализации pull модели получения данных наблюдателя
    // не используем: когда наблюдаемому объекту нужно получить от наблюдателей некоторый результат
    public class LogEntryEventArgs : EventArgs
    {
        public string LogEntry { get; internal set; }
    }
    public class LogFileReader2 : IDisposable
    {
        private readonly string _logFileReader;
        public LogFileReader2(string logFileName)
        {

        }
        public event EventHandler<LogEntryEventArgs> OnNewLogEntry;
        private void CheckFile()
        {
            foreach (var logEntry in ReadNewLogEntries())
            {
                RaiseNewLogEntries(logEntry);
            }
        }
        private void RaiseNewLogEntries(string logEntry)
        {
            var handler = OnNewLogEntry;
            if (handler != null)
                handler(this, new LogEntryEventArgs() { LogEntry = logEntry });
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    // 3. Строго типизированный наблюдатель (оперируем интерфейсом, а не набором событий)
    // Наблюдатель единственный, может переродится в полноценную зависимость
    // Используем: в качестве временной именованной зависимости для группировки набора событий
    // не используем: В открытом API  (в повторно используемом коде или на стыке модулей
    public interface ILogFileReaderObserver
    {
        void NewLogEntry(string LogEntry);
        void FileWasRolled(string oldlogFile, string newLogFile);
    }

    public class LogFileReader3 : IDisposable
    {
        private readonly ILogFileReaderObserver _observer;
        private readonly string _logFileName;

        public LogFileReader3(string logFileName, ILogFileReaderObserver observer)
        {
            _logFileName = logFileName;
            _observer = observer;
        }

        private void DetectThatNewFileWasCreated()
        {
            // Метод вызывается по таймеру
            if (NewLogFileWasCreated())
            {
                _observer.FileWasRolled(_logFileName, GetNewLogFileName());
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }




// 4 IObserver/ IObservable
// Реактивные расширения (push последовательности)
public class LogFileReader4 : IDisposable
{
    private readonly string _fileName;
    private readonly Subject<string> _logEntriesSubject;

    public LogFileReader4(string FileName)
    {
        _fileName = FileName;
    }
    public void Dispose()
    {
        CloseFile();
        _logEntriesSubject.OnComplete();
    }
    public IObservable<string> NewMessage
    {
        get
        {
            return _logEntriesSubject;
        }
    }
    private void CheckFile()
    {
        foreach (var logEntry in ReadNewLogEntries())
        {
            _logEntriesSubject.OnNext(logEntry);
        }
    }

}
}
