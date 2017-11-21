using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Назначение: определяет семейство алгоритмов, инкапсулирует каждый из них и делает их взаимозаменяемыми.
/// Стратегия позволяет изменять алгоритмы независимо от клиентов, которые ими пользуются.
/// (Стратегия инкапсулирует определенное поведение с возможностью её подмены.)
/// Мотивация: выделение поведения или алгоритма с возможностью его замены во время выполнения 
/// (Именно во время ВЫПОЛНЕНИЯ! Гибкость не бывает бесплатной Опастность наружения принципа замещения Лисков).
/// </summary>
namespace Strategy
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Создаём контекст и инициализируем его первой стратегией.
            Context context = new Context(new ConcreteStrategy1());
            // Выполняем операцию контекста, которая использует первую стратегию.
            context.ExecuteOperation();
            // Заменяем в контексте первую стратегию второй.
            context.SetStrategy(new ConcreteStrategy2());
            // Выполняем операцию контекста, которая теперь использует вторую стратегию.
            context.ExecuteOperation();
        }
    }

    // Класс реализующий конкретную стратегию, должен наследовать этот интерфейс
    // Класс контекста использует этот интерфейс для вызова конкретной стратегии
    public interface IStrategy
    {
        void Algorithm();
    }

    // Первая конкретная реализация-стратегия.
    public class ConcreteStrategy1 : IStrategy
    {
        public void Algorithm()
        {
            Console.WriteLine("Выполняется алгоритм стратегии 1.");
        }
    }

    // Вторая конкретная реализация-стратегия.
    // Реализаций может быть сколько угодно много.
    public class ConcreteStrategy2 : IStrategy
    {
        public void Algorithm()
        {
            Console.WriteLine("Выполняется алгоритм стратегии 2.");
        }
    }

    // Контекст, использующий стратегию для решения своей задачи.
    public class Context
    {
        // Ссылка на интерфейс IStrategy
        // позволяет автоматически переключаться между конкретными реализациями
        // (другими словами, это выбор конкретной стратегии).
        private IStrategy _strategy;

        // Конструктор контекста.
        // Инициализирует объект стратегией.
        public Context(IStrategy strategy)
        {
            _strategy = strategy;
        }

        // Метод для установки стратегии.
        // Служит для смены стратегии во время выполнения.
        // В C# может быть реализован также как свойство записи.
        public void SetStrategy(IStrategy strategy)
        {
            _strategy = strategy;
        }

        // Некоторая функциональность контекста, которая выбирает
        //стратегию и использует её для решения своей задачи.
        public void ExecuteOperation()
        {
            _strategy.Algorithm();
        }
    }
    object

}
