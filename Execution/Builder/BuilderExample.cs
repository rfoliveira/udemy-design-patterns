using System;
using System.Collections.Generic;
using System.Text;

namespace Execution.Builder
{
    public class HtmlElement
    {
        public string Name, Text;
        public List<HtmlElement> Elements = new List<HtmlElement>();
        public const int IndentSize = 2;

        public HtmlElement()
        {
            //
        }

        public HtmlElement(string name, string text)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Text = text ?? throw new ArgumentNullException(paramName: nameof(text));
        }

        private string ToStringImpl(int indent)
        {
            var sb = new StringBuilder();
            var i = new string(' ', IndentSize * indent);
            sb.AppendLine($"{i}<{Name}>");

            if (!string.IsNullOrWhiteSpace(Text))
            {
                sb.Append(new string(' ', IndentSize * (indent + 1)));
                sb.AppendLine(Text);
            }

            foreach (var el in Elements)
            {
                sb.Append(el.ToStringImpl(indent + 1));
            }

            sb.AppendLine($"{i}</{Name}>");

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringImpl(0);
        }
    }

    public class HtmlBuilder
    {
        private readonly string rootName;
        public HtmlElement Root = new HtmlElement();

        public HtmlBuilder(string rootName)
        {
            this.rootName = rootName;
            Root.Name = rootName;
        }

        public HtmlBuilder AddChild(string childName, string childText) // fluent way..
        {
            var element = new HtmlElement(childName, childText);
            Root.Elements.Add(element);
            return this;    // fluent way (using as builder.AddChild(...).AddChild(...) for example ...
        }

        public override string ToString()
        {
            return Root.ToString();
        }

        public void Clear()
        {
            Root = new HtmlElement { Name = rootName };
        }
    }

    // FLuent Builder inheritance with recursive Generics...
    public class Person
    {
        public string Name { get; set; }
        public string Position { get; set; }

        public class Builder : PersonJobBuilder<Builder>
        {
            //
        }

        public static Builder New => new Builder();

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
        }
    }

    public abstract class PersonBuilder
    {
        protected Person person = new Person();
        public Person Build() => person;
    }

    public class PersonInfoBuilder<T> : PersonBuilder where T : PersonInfoBuilder<T>
    {
        //protected Person person = new Person();

        public T Called(string name)
        {
            person.Name = name;
            return (T)this;
        }
    }

    //public class PersonJobBuilder: PersonInfoBuilder (before...)
    public class PersonJobBuilder<T> : PersonInfoBuilder<PersonJobBuilder<T>> where T : PersonJobBuilder<T>
    {
        // this way, we can't use like: var b = new PersonInfoBuilder().Called("Rafael").WorksAsA("developer"); 
        // because the PersonInfoBuilder doesn't know about his childs
        //public PersonJobBuilder WorksAsA(string position)
        //{
        //    person.Position = position;
        //    return this;
        //}

        public T WorksAsA(string position)
        {
            person.Position = position;
            return (T)this;
        }
    }

    //Faceted builder
    public class Person2
    {
        //address
        public string StreetAddresss, Postcode, City;

        //employment
        public string CompanyName, Position;
        public int AnnualIncome;

        public override string ToString()
        {
            return $"{nameof(StreetAddresss)}: {StreetAddresss}, " +
                $"{nameof(Postcode)}: {Postcode}, " +
                $"{nameof(City)}: {City}, " +
                $"{nameof(CompanyName)}: {CompanyName}, " +
                $"{nameof(Position)}: {Position}, " +
                $"{nameof(AnnualIncome)}: {AnnualIncome}";
        }
    }

    public class PersonBuilder2  //facade
    {
        protected Person2 person = new Person2();

        public PersonJobBuilder2 Works => new PersonJobBuilder2(person);
        public PersonAddressBuilder Lives => new PersonAddressBuilder(person);

        // Com o uso de um operador implicit de Person, este método não é mais necessário
        //public override string ToString()
        //{
        //    return person.ToString();
        //}

        public static implicit operator Person2(PersonBuilder2 personBuilder)
        {
            return personBuilder.person;
        }
    }

    public class PersonJobBuilder2 : PersonBuilder2
    {
        public PersonJobBuilder2(Person2 person)
        {
            this.person = person;
        }

        public PersonJobBuilder2 At(string companyName)
        {
            person.CompanyName = companyName;
            return this;
        }

        public PersonJobBuilder2 AsA(string position)
        {
            person.Position = position;
            return this;
        }

        public PersonJobBuilder2 Earning(int amount)
        {
            person.AnnualIncome = amount;
            return this;
        }

        // Não precisa mais fazer assim porque a classe pai está expondo implicidademente o objeto person
        // dessa forma quando o ToString é chamado este é executado de person, não da classe em si
        //public override string ToString()
        //{
        //    return base.ToString();
        //}
    }

    public class PersonAddressBuilder : PersonBuilder2
    {
        public PersonAddressBuilder(Person2 person)
        {
            this.person = person;
        }

        public PersonAddressBuilder At(string streetAddress)
        {
            person.StreetAddresss = streetAddress;
            return this;
        }

        public PersonAddressBuilder WithPostcode(string postCode)
        {
            person.Postcode = postCode;
            return this;
        }

        public PersonAddressBuilder In(string city)
        {
            person.City = city;
            return this;
        }

        // Não precisa mais fazer assim porque a classe pai está expondo implicidademente o objeto person
        // dessa forma quando o ToString é chamado este é executado de person, não da classe em si
        //public override string ToString()
        //{
        //    return base.ToString();
        //}
    }
}
