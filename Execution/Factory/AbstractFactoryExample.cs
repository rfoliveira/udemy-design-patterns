using System;
using System.Collections.Generic;
using static System.Console;

namespace Execution.Factory
{
    public interface IHotDrink
    {
        void Consume();
    }

    internal class Tea : IHotDrink
    {
        public void Consume()
        {
            WriteLine("This tea is nice but I'd prefer it with milk.");
        }
    }

    internal class Coffee : IHotDrink
    {
        public void Consume()
        {
            WriteLine("This coffe is delicious!");
        }
    }

    public interface IHotDrinkFactory
    {
        IHotDrink Prepare(int amount);
    }

    public class TeaFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            WriteLine($"Put in tea bag, boil water, pour {amount} ml, add lemon, enjoy!");
            return new Tea();
        }
    }

    public class CoffeeFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            WriteLine($"Grind some beans, boil water, pour {amount} ml, add cream and sugar, enjoy!");
            return new Coffee();
        }
    }

    public class HotDrinkMachine
    {
        // violates the open closed principle
        // because if we have a new type of drink, this enumeration wneeds to be changed...
        public enum AvaliableDrink
        {
            Tea, Coffee
        }

        private Dictionary<AvaliableDrink, IHotDrinkFactory> factories =
            new Dictionary<AvaliableDrink, IHotDrinkFactory>();

        public HotDrinkMachine()
        {
            foreach (AvaliableDrink drink in Enum.GetValues(typeof(AvaliableDrink)))
            {
                var factory = (IHotDrinkFactory)Activator.CreateInstance(
                    Type.GetType("Execution.Factory." + Enum.GetName(typeof(AvaliableDrink), drink) + "Factory")
                );
                factories.Add(drink, factory);
            }
        }

        public IHotDrink MakeDrink(AvaliableDrink drink, int amount)
        {
            return factories[drink].Prepare(amount);
        }
    }

    //To avoid OCP, just write a new constructor that use reflection...
    public class HotDrinkMachineWithOCP
    {
        private List<Tuple<string, IHotDrinkFactory>> namedFactories =
            new List<Tuple<string, IHotDrinkFactory>>();

        public HotDrinkMachineWithOCP()
        {
            foreach (var t in typeof(HotDrinkMachine).Assembly.GetTypes())
            {
                if (typeof(IHotDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    namedFactories.Add(Tuple.Create(
                       t.Name.Replace("Factory", string.Empty), (IHotDrinkFactory)Activator.CreateInstance(t)));
                }
            }
        }

        public IHotDrink MakeDrink()
        {
            WriteLine("Avaliable drinks:");
            for (int i = 0; i < namedFactories.Count; i++)
            {
                var tuple = namedFactories[i];
                WriteLine($"{i}: {tuple.Item1}");
            }

            while (true)
            {
                string s;                

                if ((s = ReadLine()) != null
                    && int.TryParse(s, out int i)
                    && i >= 0
                    && i < namedFactories.Count)
                {
                    Write("Specify amount: ");
                    s = ReadLine();

                    if (s != null
                        && int.TryParse(s, out int amount)
                        && amount > 0)
                    {
                        return namedFactories[i].Item2.Prepare(amount);
                    }
                }
                WriteLine("Incorrect input, try again!");
            }
        }
    }
}
