using MoreLinq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static System.Console;

namespace Execution.Adapter
{
    // this class represents the Main method of the lesson
    // initially, this class only draw based on Points, not Lines
    // The challlenge here is using Adapter to make it draw based on lines, 
    // not only points
    public static class AdapterExamples
    {
        private static readonly List<VectorObject> vectorObjects =
            new List<VectorObject>
            {
                new VectorRectangle(1,1,10,10),
                new VectorRectangle(3,3,6,6)
            };

        // Method to be adapted...
        public static void DrawPoint(Point2 p)
        {
            Write(".");
        }

        // rendering with Adapter pattern
        public static void Draw()
        {
            foreach (var vo in vectorObjects)
            {
                foreach (var line in vo)
                {
                    var adapter = new LineToPointAdapter(line);
                    adapter.ForEach(DrawPoint);
                }
            }
        }

        public static void DrawWithCaching()
        {
            foreach (var vo in vectorObjects)
            {
                foreach (var line in vo)
                {
                    var adapter = new LineToPointAdapterWithCache(line);
                    adapter.ForEach(DrawPoint);
                }
            }
        }
    }

    public class Point2
    {
        public int X, Y;

        public Point2(int x, int y)
        {
            X = x;
            Y = y;
        }

        // Equals and GetHashCode will be used in cache approach...
        public override bool Equals(object obj)
        {
            var point = obj as Point2;
            return point != null &&
                   X == point.X &&
                   Y == point.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
        }
    }

    public class Line
    {
        public Point2 Start, End;

        public Line(Point2 start, Point2 end)
        {
            Start = start ?? throw new ArgumentNullException(nameof(start));
            End = end ?? throw new ArgumentNullException(nameof(end));
        }

        // Equals and GetHashCode will be used in cache approach...
        public override bool Equals(object obj)
        {
            var line = obj as Line;
            return line != null &&
                   EqualityComparer<Point2>.Default.Equals(Start, line.Start) &&
                   EqualityComparer<Point2>.Default.Equals(End, line.End);
        }

        public override int GetHashCode()
        {
            var hashCode = -1676728671;
            hashCode = hashCode * -1521134295 + EqualityComparer<Point2>.Default.GetHashCode(Start);
            hashCode = hashCode * -1521134295 + EqualityComparer<Point2>.Default.GetHashCode(End);
            return hashCode;
        }
    }

    public class VectorObject: Collection<Line>
    {
        //
    }

    // Example of use of VectorObject
    public class VectorRectangle: VectorObject
    {
        public VectorRectangle(int x, int y, int width, int height)
        {
            Add(new Line(new Point2(x, y), new Point2(x + width, y)));
            Add(new Line(new Point2(x + width, y), new Point2(x + width, y + height)));
            Add(new Line(new Point2(x, y), new Point2(x, y + height)));
            Add(new Line(new Point2(x, y + height), new Point2(x + width, y + height)));
        }
    }

    // Class to do the Adapter pattern
    public class LineToPointAdapter : Collection<Point2>
    {
        private static int count;

        public LineToPointAdapter(Line line)
        {
            WriteLine($"{++count}: Generating points for the line [{line.Start.X},{line.Start.Y}]-[{line.End.X},{line.End.Y}]");

            int left = Math.Min(line.Start.X, line.End.X);
            int right = Math.Max(line.Start.X, line.End.X);
            int top = Math.Min(line.Start.Y, line.End.Y);
            int bottom = Math.Max(line.Start.Y, line.End.Y);
            int dx = right - left;
            int dy = line.End.Y - line.Start.Y;

            if (dx == 0)
            {
                for (int y = top; y <= bottom; ++y)
                {
                    Add(new Point2(left, y));
                }
            }
            else if (dy == 0)
            {
                for (int x = left; x <= right; ++x)
                {
                    Add(new Point2(x, top));
                }
            }
        }
    }

    // with caching... 
    public class LineToPointAdapterWithCache : IEnumerable<Point2>
    {
        private static int count;
        private Dictionary<int, List<Point2>> cache = new Dictionary<int, List<Point2>>();

        public LineToPointAdapterWithCache(Line line)
        {
            var hash = line.GetHashCode();
            if (cache.ContainsKey(hash)) return;

            WriteLine($"{++count}: Generating points for the line [{line.Start.X},{line.Start.Y}]-[{line.End.X},{line.End.Y}]");

            var points = new List<Point2>();

            int left = Math.Min(line.Start.X, line.End.X);
            int right = Math.Max(line.Start.X, line.End.X);
            int top = Math.Min(line.Start.Y, line.End.Y);
            int bottom = Math.Max(line.Start.Y, line.End.Y);
            int dx = right - left;
            int dy = line.End.Y - line.Start.Y;

            if (dx == 0)
            {
                for (int y = top; y <= bottom; ++y)
                {
                    points.Add(new Point2(left, y));
                }
            }
            else if (dy == 0)
            {
                for (int x = left; x <= right; ++x)
                {
                    points.Add(new Point2(x, top));
                }
            }

            cache.Add(hash, points);
        }

        public IEnumerator<Point2> GetEnumerator()
        {
            return cache.Values.SelectMany(x => x).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class PointAdapter
    {
        public int X, Y;

        public PointAdapter(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
