
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Execution.Adapter
{
    public interface IRectangle
    {
        int Width { get;  }
        int Height { get; }
    }

    public class Square
    {
        public int Side;
    }

    public static class ExtensionMethods
    {
        public static int Area(this IRectangle rc)
        {
            return rc.Width * rc.Height;
        }
    }

    public class AdapterExercise : IRectangle
    {
        private readonly Square square;

        public AdapterExercise(Square square)
        {
            this.square = square;    
        }

        public int Width => square.Side;
        public int Height => square.Side;
    }
}
