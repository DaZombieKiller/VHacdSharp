using System.Collections.Generic;

namespace VHacdSharp
{
    public class ConvexDecomposition
    {
        public IReadOnlyList<ConvexHull> ConvexHulls { get; }
        public Vector3D CenterOfMass { get; }

        public ConvexDecomposition(IReadOnlyList<ConvexHull> convexHulls, Vector3D centerOfMass)
        {
            ConvexHulls  = convexHulls;
            CenterOfMass = centerOfMass;
        }
    }
}
