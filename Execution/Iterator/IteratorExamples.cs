using System;
using static System.Console;

namespace Execution.Iterator
{
    public class Node<T>
    {
        public T Value;
        public Node<T> Left, Right;
        public Node<T> Parent;

        public Node(T value)
        {
            this.Value = value;
        }

        public Node(T value, Node<T> left, Node<T> right)
        {
            this.Value = value;
            this.Left = left;
            this.Right = right;

            left.Parent = right.Parent = this;
        }
    }

    public class InOrderIterator<T>
    {
        public Node<T> Current { get; set; } // the node that iterator points
        private readonly Node<T> root;
        private bool yieldedStart;

        public InOrderIterator(Node<T> root)
        {
            this.root = Current = root;

            while (Current.Left != null)
                Current = Current.Left;

            //   1   <- root 
            //  / \
            // 2   3
            // ^ Current
        }

        public bool MoveNext()
        {
            if (!yieldedStart)
            {
                yieldedStart = true;
                return true;
            }

            if (Current.Right != null)
            {
                Current = Current.Right;

                while (Current.Left != null)
                    Current = Current.Left;

                return true;
            }
            else
            {
                var p = Current.Parent;

                while (p != null && Current == p.Right)
                {
                    Current = p;
                    p = p.Parent;
                }

                Current = p;

                return Current != null;
            }
        }

        public void Reset()
        {
            Current = root;
            yieldedStart = true;
        }
    }

    public class IteratorExamples
    {
        public static void Demo1()
        {
            //   1
            //  / \
            // 2   3

            // in-order: 213

            var root = new Node<int>(
                1,
                new Node<int>(2),
                new Node<int>(3)
            );

            var it = new InOrderIterator<int>(root);

            while (it.MoveNext())
            {
                Write(it.Current.Value);
                Write(',');
            }

            WriteLine();
        }
    }
}
