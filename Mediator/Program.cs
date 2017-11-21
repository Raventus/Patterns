using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator
{
    /// <summary>
    /// Посредник определяет объект, инкапсулирующий способ взаимодействия множества объектов
    /// Клей, связывающий несколько независимых классов между собой. Он избавляет классы от необходимости ссылаться друг на друга , позволяя тем самым независимо
    /// изменять их и анализировать
    /// </summary>
    /// Если классы сильно связаны, это мешает повторно использовать их в другом контексте
    /// Используется в поддатливом дизайне (решает проблему преждевременного обобщения
    /// Явный и неявный посредник (Я) - об посреднике знают компоненты (Н) - Объединяет компоненты без их ведома (агрегатор событий (Event Aggregator))
    /// Слабая связность на границах модулей(зацепление coupling), а внутри необходима сильная (cohesion)
    /// Если классы должны изменяться всегда вместе (экспорт/ импорт),то использование посредника излишне 
    class Program
    {
        static void Main(string[] args)
        {
            ConcretaMediator m = new ConcretaMediator();

            ConcreteColleague1 c1 = new ConcreteColleague1(m);
            ConcreteColleague2 c2 = new ConcreteColleague2(m);

            m.Colleague1 = c1;
            m.Colleague2 = c2;

            c1.Send("How are you?");
            c2.Send("Fine, thanks");
        }


    }
    abstract class Mediator
    {
        public abstract void Send(string message, Colleague colleague);
    }

    class ConcretaMediator : Mediator
    {
        public ConcreteColleague1 Colleague1 { private get; set; }
        public ConcreteColleague2 Colleague2
        {
            private get; set;
        }



        public override void Send(string message, Colleague colleague)
        {
            if (colleague == Colleague1)
            {
                Colleague2.Notify(message);
            }
            else
                Colleague1.Notify(message);
        }
    }

    abstract class Colleague
    {
        protected Mediator mediator;
        public Colleague(Mediator mediator)
        {
            this.mediator = mediator;
        }
    }

    class ConcreteColleague1 : Colleague
    {
        public ConcreteColleague1(Mediator mediator) : base(mediator)
        {
        }

        public void Send(string message)
        {
            mediator.Send(message, this);
        }
        public void Notify(string message)
        {
            Console.WriteLine("Colleague1 gets message: " + message);
        }
    }

    class ConcreteColleague2 : Colleague
    {
        public ConcreteColleague2(Mediator mediator)
          : base(mediator)
        {
        }

        public void Send(string message)
        {
            mediator.Send(message, this);
        }

        public void Notify(string message)
        {
            Console.WriteLine("Colleague2 gets message: " + message);
        }
    }

}

