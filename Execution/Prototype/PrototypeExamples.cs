using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Execution.Prototype
{
    /*
        ICloneable isn't a good approach
     */
    // public class Address: ICloneable {

    //     public readonly string StreetName;
    //     public int HouseNumber;

    //     public Address(string streetName, int houseNumber)
    //     {
    //         StreetName = streetName;
    //         HouseNumber = houseNumber;
    //     }

    //     public override string ToString() {
    //         return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
    //     }

    //     public object Clone()
    //     {
    //         return new Address(StreetName, HouseNumber);
    //     }
    // }

    // public class Person: ICloneable {

    //     public readonly string[] Names;
    //     public readonly Address Address;

    //     public Person(string[] names, Address address)
    //     {
    //         Names = names;
    //         Address = address;
    //     }

    //     public override string ToString() {
    //         return $"{nameof(Names)}: {string.Join(",", Names)}, {nameof(Address)}: {Address.ToString()}";
    //     }

    //     public object Clone() {
    //         return new Person(Names, Address);
    //     }
    // }

    // using copy constructor..
    public class Address
    {
        public string StreetName, City, Country;

        public Address(string streetName, string city, string country)
        {
            StreetName = streetName ?? throw new ArgumentNullException(paramName: nameof(streetName));
            City = city ?? throw new ArgumentNullException(paramName: nameof(city));
            Country = country ?? throw new ArgumentNullException(paramName: nameof(country));
        }

        public Address(Address other)
        {
            StreetName = other.StreetName;
            City = other.City;
            Country = other.Country;
        }

        public override string ToString()
        {
            return $"{nameof(StreetName)}: {StreetName}, {nameof(City)}: {City}, {nameof(Country)}: {Country}";
        }
    }

    public class Employee
    {
        public string Name;
        public Address Address;

        public Employee(string name, Address address)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Address = address ?? throw new ArgumentNullException(paramName: nameof(address));
        }

        public Employee(Employee other)
        {
            Name = other.Name;
            Address = new Address(other.Address);
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Address)}: {Address.ToString()}";
        }
    }

    public static class ExtensionMethods
    {
        public static T DeepCopy<T>(this T self)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(ms, self);
            ms.Seek(0, SeekOrigin.Begin);

            object copy = bf.Deserialize(ms);
            ms.Close();

            return (T)copy;
        }

        public static T DeepCopyXml<T>(this T self)
        {
            using (var ms = new MemoryStream())
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                xs.Serialize(ms, self);
                ms.Position = 0;

                return (T)xs.Deserialize(ms);
            }
        }
    }

    [Serializable]  // this is unfortunatelly required
    public class Foo
    {
        public uint Stuff;
        public string Whatever;

        public override string ToString()
        {
            return $"{nameof(Stuff)}: {Stuff}, {nameof(Whatever)}: {Whatever}";
        }
    }
}
