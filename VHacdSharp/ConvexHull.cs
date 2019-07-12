using System.Collections.Generic;

namespace VHacdSharp
{
    public class ConvexHull
    {
        public IReadOnlyList<double> Points { get; internal set; }
        public IReadOnlyList<uint> Triangles { get; internal set; }
        public double Volume { get; internal set; }
        public Vector3D Center { get; internal set; }
    }
}
