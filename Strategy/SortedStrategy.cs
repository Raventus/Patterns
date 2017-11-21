using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategy
{
    class SortedStrategy
    {
    }

    class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return String.Format("Id = {0}, Name = {1}", Id, Name);
        }
    }

    class EmployeeByIdComparer : IComparer<Employee>
    {
        public int Compare(Employee x, Employee y)
        {
            return x.Id.CompareTo(y.Id);
        }

        public static void SortLists()
        {
            var list = new List<Employee>();
            // используем "функтор"
            list.Sort(new EmployeeByIdComparer());
            // используем делегат
            list.Sort((x, y) => x.Name.CompareTo(y.Name));
        }
    }
}
