using System;
using System.Collections.Generic;
namespace consoleApp
{
    class DelegateLambda
    {
        public DelegateLambda(string name, int age, decimal salary)
        {
            Name = name;
            Age = age;
            Salary = salary;
        }
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; } 
    }

    class EmployeeFilterByAge
    {
        int m_age;
        public EmployeeFilterByAge(int age)
        {
            m_age = age;
        }
        public bool OlderThan(DelegateLambda employee)
        {
            return employee.Age > m_age;
        }
    }

    class TestLambda
    {
        public static void Test()
        {
            List<DelegateLambda> employees = new List<DelegateLambda>();
            employees.Add(new DelegateLambda("John", 33, 22000m));
            employees.Add(new DelegateLambda("Eric", 42, 18000m));
            employees.Add(new DelegateLambda("Michael", 32, 19500m));
            int ageThreshold = 40;
            Predicate<DelegateLambda> match = e => e.Age > ageThreshold;
            ageThreshold = 30;
            int index = employees.FindIndex(match);
            Console.WriteLine(index);

            Console.WriteLine(employees.FindIndex(e => e.Age > 40));
        }
    }

    class TestAnonymousMethods
    {
        public static void Test()
        {
            List<DelegateLambda> employees = new List<DelegateLambda>();
            employees.Add(new DelegateLambda("John", 33, 22000m));
            employees.Add(new DelegateLambda("Eric", 42, 18000m));
            employees.Add(new DelegateLambda("Michael", 33, 19500m));
            EmployeeFilterByAge filterByAge = new EmployeeFilterByAge(40);
            int index = employees.FindIndex(filterByAge.OlderThan);
            Console.WriteLine(index);

            Console.WriteLine(employees.FindIndex(delegate(DelegateLambda employee)
            {
                return employee.Age > 40;
            }));
        }
    }
}