using System;

namespace Execution.Factory
{
    public enum CoordinateSystem
    {
        Cartesian, Polar
    }

    public class Point
    {
        private double x, y;

        /// <summary>
        /// initializes a point from EITHER cartesian or polar systems (example without Factory method implementation...)
        /// </summary>
        /// <param name="a">x if cartesian, rho if polar....</param>
        /// <param name="b"></param>
        /// <param name="system"></param>
        public Point(double a, double b, CoordinateSystem system = CoordinateSystem.Cartesian)
        {
            switch (system)
            {
                case CoordinateSystem.Cartesian:
                    x = a;
                    y = b;
                    break;
                case CoordinateSystem.Polar:
                    x = a * Math.Cos(b);
                    y = a * Math.Sin(b);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(system), system, null);
            }
        }

        /*
         * Using Factory Method
        */
        //private Point(double x, double y)
        //{
        //    this.x = x;
        //    this.y = y;
        //}

        //public Point(double x, double y)
        //{
        //    this.x = x;
        //    this.y = y;
        //}

        private Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // factory method ...
        // BUT this way hurts the SRP concept....
        //public static Point NewCartesianPoint(double x, double y)
        //{
        //    return new Point(x, y);
        //}

        //public static Point NewPolarPoint(double rho, double theta)
        //{
        //    return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        //}

        public override string ToString()
        {
            return $"{nameof(x)}: {x}, {nameof(y)}: {y}";
        }

        // inner class to use Factory as static and can't use Point instantiation by "new"
        public static class Factory
        {
            public static Point NewCartesianPoint(double x, double y)
            {
                return new Point(x, y);
            }

            public static Point NewPolarPoint(double rho, double theta)
            {
                return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }
        }

        public static Point Origin = new Point(0, 0);
    }

    // to avoid crashes the SRP just make a factory class with the methods in separate
    // but in this manner, the constructor has to be "public"
    // or it could be a singleton...
    public class PointFactory
    {
        public static Point NewCartesianPoint(double x, double y)
        {
            return new Point(x, y);
        }

        public static Point NewPolarPoint(double rho, double theta)
        {
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }
    }
}
