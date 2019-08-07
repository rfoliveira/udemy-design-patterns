using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Execution.Bridge
{
    class BridgeExercise
    {
    }

    public interface IRenderer2
    {
        string WhatToRenderAs { get; }
    }

    public abstract class Shape2
    {
        private IRenderer2 renderer;

        public string Name { get; set; }
        protected Shape2(IRenderer2 renderer)
        {
            this.renderer = renderer;
        }

        public override string ToString()
        {
            return $"Drawing {Name} as {renderer.WhatToRenderAs}";
        }
    }

    public class Triangle : Shape2
    {
        public Triangle(IRenderer2 renderer) : base(renderer) { Name = "Triangle"; }
    }

    public class Square : Shape2
    {
        public Square(IRenderer2 renderer) : base(renderer) { Name = "Square"; }
    }

    public class VectorSquare : Square
    {
        public VectorSquare(IRenderer2 renderer) : base(renderer) { }   // all class inherited of Shape needs this constructor, otherwise doesn't work as such
        public override string ToString() => $"Drawing {Name} as lines";
    }

    public class RasterSquare : Square
    {
        public RasterSquare(IRenderer2 renderer) : base(renderer) { }
        public override string ToString() => $"Drawing {Name} as pixels";
    }

    // imagine VectorTriangle and RasterTriangle are here too

    public class VectorRenderer2 : IRenderer2
    {
        public string WhatToRenderAs => "lines";
    }

    public class RasterRenderer2 : IRenderer2
    {
        public string WhatToRenderAs => "pixels";
    }
}
