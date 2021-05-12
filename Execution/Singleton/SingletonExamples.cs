using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Console;

namespace Execution.Singleton
{
    public interface IDatabase
    {
        int GetPopulation(string name);
    }

    public class SingletonDatabase : IDatabase
    {
        private Dictionary<string, int> capitals;
        private static int instanceCount;
        public static int Count => instanceCount;

        // another way...
        // to explain test problem with singletons...
        //private static Lazy<SingletonDatabase> instance = new Lazy<SingletonDatabase>(() => new SingletonDatabase());
        //laziness + thread safety
        private static Lazy<SingletonDatabase> instance = new Lazy<SingletonDatabase>(() => {
            instanceCount++;
            return new SingletonDatabase();
        });
        public static IDatabase Instance => instance.Value;

        private SingletonDatabase()
        {
            //instanceCount++;
            WriteLine("Initializing database...");

            capitals = File.ReadAllLines(
                Path.Combine(
                    new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName,
                    "capitals.txt"
                )
            )
            .Batch(2)
            .ToDictionary(
                list => list.ElementAt(0).Trim(),
                list => int.Parse(list.ElementAt(1))
            );
        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }
    }

    // This class show the problem with using a Singleton approach. 
    // The problem is because this classe needs to hardcoded the SingletonDatabase
    public class SingletonRecordFinder
    {
        public int GetTotalCalculation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (var name in names)
            {
                result += SingletonDatabase.Instance.GetPopulation(name);   // this is a bad ideia...
            }

            return result;
        }
    }

    // Right way to use the aproch below
    public class ConfigurablerecordFinder
    {
        private IDatabase database;

        public ConfigurablerecordFinder(IDatabase database)
        {
            this.database = database ?? throw new ArgumentNullException(paramName: nameof(database));
        }

        public int GetTotalCalculation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (var name in names)
                result += database.GetPopulation(name);   // the right way...

            return result;
        }
    }

    public class DummyDatabase : IDatabase
    {
        public int GetPopulation(string name)
        {
            return new Dictionary<string, int>
            {
                ["Alpha"] = 1,
                ["Betha"] = 2,
                ["Gamma"] = 3
            }[name];
        }
    }

    // example of use of dependency injection
    // this class isn't a singleton, but behaves as such
    public class OrdinaryDatabase : IDatabase
    {
        private Dictionary<string, int> capitals;

        public OrdinaryDatabase()
        {
            WriteLine("Initializing database...");

            capitals = File.ReadAllLines(
                Path.Combine(
                    new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName,
                    "capitals.txt"
                )
            )
            .Batch(2)
            .ToDictionary(
                list => list.ElementAt(0).Trim(),
                list => int.Parse(list.ElementAt(1))
            );
        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }
    }
}
