using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Execution.Adapter
{
    // For use of VectorExample, we need to make an interface
    // to yield the dimension of Vector
    public interface IInteger
    {
        int Value { get; }
    }

    public static class Dimensions
    {
        public class Two : IInteger
        {
            public int Value => 2;
        }

        public class Three : IInteger
        {
            public int Value => 3;
        }
    }    

    // T = type, D = dimension
    public class VectorExample<T, D> where D: IInteger, new()
    {
        protected T[] data;

        public VectorExample()
        {
            // it doesnt work without an interface
            data = new T[new D().Value];
        }

        public VectorExample(params T[] values)
        {
            var requiredSize = new D().Value;
            data = new T[requiredSize];

            var providedSize = values.Length;

            for (int i = 0; i < Math.Min(requiredSize, providedSize); ++i)
            {
                data[i] = values[i];
            }
        }

        public T this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }

        public T X
        {
            get => data[0];
            set => data[0] = value;
        }
    }

    // example of use of VectoExample class
    // for vector of two integers
    // one approach
    public class Vector2i: VectorExample<int, Dimensions.Two>
    {
        public Vector2i()
        {

        }

        public Vector2i(params int[] values): base(values)
        {

        }
    }

    // update on Vector2i class to avoid use operators
    /*
     * Example of use:
     * var v = new Vector2i(1, 2);
     * v[0] = 0;
     * 
     * var vv = new Vector2i(2, 3);
     * var result = v + vv; // this will work
     *
     */
    public class Vector2iWithOperator: VectorOfInt<Dimensions.Two>
    {
        public Vector2iWithOperator() { }
        public Vector2iWithOperator(params int[] values): base(values) { }
    }

    // To make possible add operations on vector class
    public class VectorOfInt<D>: VectorExample<int, D> where D: IInteger, new()
    {
        public VectorOfInt() { }
        public VectorOfInt(params int[] values): base(values) { }

        public static VectorOfInt<D> operator +(VectorOfInt<D> leftSide, VectorOfInt<D> rightSide)
        {
            var result = new VectorOfInt<D>();
            var dim = new D().Value;

            for (int i = 0; i < dim; i++)
            {
                result[i] = leftSide[i] + rightSide[i];
            }

            return result;
        }
    }
}
