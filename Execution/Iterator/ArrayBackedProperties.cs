using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Execution.Iterator
{
    public class Creature: IEnumerable<int>
    {
        private int[] stats = new int[3];
        private const int strength = 0;

        public int Strength { 
            get => stats[strength];
            set => stats[strength] = value;
        }

        public int Agility { get; set; }
        public int Intelligence { get; set; }

        //public double AverageStat => (Strength + Agility + Intelligence) / 3.0;
        public double AverageStats => stats.Average();

        public IEnumerator<int> GetEnumerator()
        {
            return stats.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        // Make this class an observable collection
        public int this[int index]
        {
            get => stats[index];
            set => stats[index] = value;
        }
    }

    public class IteratorArrayBackedPropertiesExample
    { 
        public static void Demo()
        {

        }
    }

}
