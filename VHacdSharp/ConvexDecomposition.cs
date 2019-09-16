using System.Collections.Generic;

namespace VHacdSharp
{
    public class ConvexDecomposition
    {
        public IReadOnlyList<ConvexHull> ConvexHulls { get; }
        public TVector3<double> CenterOfMass { get; }

        public ConvexDecomposition(IReadOnlyList<ConvexHull> convexHulls, TVector3<double> centerOfMass)
        {
            ConvexHulls  = convexHulls;
            CenterOfMass = centerOfMass;
        }
    }
}
