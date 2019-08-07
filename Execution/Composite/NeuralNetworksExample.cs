using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Execution.Composite
{
    // to mantain the recursiveness and the possibility to connect a neuron or collection or list of neurons 
    // to another neuron object or list or collection
    public static class NeuronExtensionMethods
    {
        public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
        {
            if (ReferenceEquals(self, other))
                return;

            foreach (var from in self)
            {
                foreach (var to in other)
                {
                    from.Out.Add(to);
                    to.In.Add(from);
                }
            }
        }
    }

    //public class Neuron
    public class Neuron: IEnumerable<Neuron>
    {
        public float Value;
        public List<Neuron> In, Out; 

        public void ConnectTo(Neuron other)
        {
            Out.Add(other);
            other.In.Add(this);
        }

        public IEnumerator<Neuron> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    // the connectivity still required!
    public class NeuronLayer: Collection<Neuron>
    {

    }

    // noooo
    // with NeuronExtensionMethod it's works...
    public class NeuronRing: List<Neuron>
    {

    }

    public static class NeuralNetworksExample
    {
        public static void Demo1()
        {
            var neuron1 = new Neuron();
            var neuron2 = new Neuron();

            neuron1.ConnectTo(neuron2);
        }

        public static void Demo2()
        {
            var neuron1 = new Neuron();
            var neuron2 = new Neuron();

            neuron1.ConnectTo(neuron2);

            var layoer1 = new NeuronLayer();
            var layoer2 = new NeuronLayer();

            // now we can mantain que connectivity besides neuron objects or list 
            neuron1.ConnectTo(layoer1);
            layoer1.ConnectTo(layoer2);
        }
    }
}
