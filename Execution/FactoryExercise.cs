using System;
using System.Collections.Generic;

namespace Execution.Factory
{
    public class PersonInFactory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Person: Id = {Id}, Name = {Name}";
        }
    }

    public interface IPersonFactory
    {
        PersonInFactory CreatePerson(string name);
    }

    public class PersonFactory: IPersonFactory
    {
        private List<PersonInFactory> personList = new List<PersonInFactory>();

        public PersonInFactory CreatePerson(string name)
        {
            var person = new PersonInFactory
            {
                Name = name,
                Id = personList.Count
            };

            personList.Add(person);
            return person;
        }

        public void Show()
        {
            personList.ForEach(p =>
            {
                Console.WriteLine(p);
            });
        }
    }
}
