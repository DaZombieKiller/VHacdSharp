using System.Collections.Generic;

namespace VHacdSharp
{
    public readonly struct ConvexHull
    {
        public IReadOnlyList<TVector3<double>> Vertices { get; }
        public IReadOnlyList<uint> Indices { get; }
        public TVector3<double> Center { get; }
        public double Volume { get; }

        public ConvexHull(IReadOnlyList<TVector3<double>> vertices, IReadOnlyList<uint> indices, double volume, TVector3<double> center)
        {
            Vertices = vertices;
            Indices  = indices;
            Center   = center;
            Volume   = volume;
        }
    }
}
