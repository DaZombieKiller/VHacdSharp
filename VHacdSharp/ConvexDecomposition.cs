using System.Collections.Generic;

namespace VHacdSharp
{
    public class ConvexDecomposition
    {
        public IReadOnlyList<ConvexHull> ConvexHulls { get; internal set; }
        public Vector3D CenterOfMass { get; internal set; }
    }
}
