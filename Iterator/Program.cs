using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iterator
{
    // Представляет доступ ко всем элементам составного объекта , не раскрывая его внутреннего представления
    // MoveNext  - переход на следующий элемент агрегата. false - достигнут конец последовательности
    /// <summary>
    /// Current - возвращает текущий элемент
    /// Reset - возвращает итератор к началу агрегата (опционально)
    /// При обновлении коллекции все итераторы становятся недействительными
    /// Внешний итератор (pull-base) процессом обхода явно управляет клиент путём вызова Next
    /// Внутрений итератор (push-base) итератор, которому передается метод обратного вызова, и он сам уведомляет клиента о "посещении" след. елемента
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Изменяемые значимые типы
            //var x = new { Items = new List<int> { 1, 2, 3 }.GetEnumerator() };
            //while (x.Items.MoveNext())
            //    Console.WriteLine(x.Items);
            //Console.ReadKey();

            ConcreteAggregate a = new ConcreteAggregate();
            a[0] = "Item A";
            a[1] = "Item B";
            a[2] = "Item C";
            a[3] = "Item D";
            ConcreteIterator i = new ConcreteIterator(a);

            Console.WriteLine("Iterating over collection:");
            object item = i.First();
            while (!i.IsDone())
            {
                Console.WriteLine(item);
                item = i.Next();
            }

            // Wait for user
            Console.ReadKey();
        }
        abstract class Aggregate
        {
            public abstract Iterator CreateIterator();
            public abstract int Count { get; protected set; }
            public abstract object this[int index] { get; set; }
        }

        class ConcreteAggregate : Aggregate
        {
            private readonly ArrayList _items = new ArrayList();

            public override object this[int index]
            {
                get => _items[index];
                set { _items.Insert(index, value); }
            }

            public override int Count
            {
                get => _items.Count;
                protected set { }
            }

            public override Iterator CreateIterator()
            {
                return new ConcreteIterator(this);
            }
        }
        abstract class Iterator
        {
            public abstract object First();
            public abstract object Next();
            public abstract bool IsDone();
            public abstract object CurrentItem();
        }

        class ConcreteIterator : Iterator
        {
            private readonly Aggregate _aggregate;
            private int _current;

            public override object CurrentItem()
            {
                return _aggregate[_current];
            }

            public ConcreteIterator(Aggregate aggregate)
            {
                this._aggregate = aggregate;
            }

            public override object First()
            {
                return _aggregate[0];
            }

            public override bool IsDone()
            {
                return _current >= _aggregate.Count;
            }

            public override object Next()
            {
                object ret = null;

                _current++;

                if (_current < _aggregate.Count)
                {
                    ret = _aggregate[_current];
                }

                return ret;
            }
        }
    }

  



    /// <summary>
    /// Пример ленивости итераторов. ошибка произойдёт в точке 2
    /// в с# 5.0 в цикле foreach переменная current внесена во внутреннюю область видимости  
    /// </summary>
    public class LazyIterator
    {
        public static IEnumerable<string> ReadFromFile(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            foreach (string line in File.ReadLines(path))
            {
                yield return line;
            }
        }
        public void LazyCheck()
        {
            var result = ReadFromFile(null); // 1
            foreach (var l in result)
            {
                Console.WriteLine(l.ToString()); // 2
            }
        }

        // Использование блока итераторов для генерации бесконечной последовательности Фибоначчи
        public static IEnumerable<int> GenerateFibonacci()
        {
            int prev = 0;
            int current = 1;
            while (true)
            {
                yield return current;
                int tmp = current;
                current = prev + current;
                prev = tmp;
            }
        }

    }
}
