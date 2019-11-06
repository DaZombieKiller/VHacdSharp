using System.Collections.Generic;

namespace VHacdSharp
{
    public readonly struct ConvexHull
    {
        public IReadOnlyList<Vector3D> Vertices { get; }
        public IReadOnlyList<uint> Indices { get; }
        public Vector3D Center { get; }
        public double Volume { get; }

        public ConvexHull(IReadOnlyList<Vector3D> vertices, IReadOnlyList<uint> indices, double volume, Vector3D center)
        {
            Vertices = vertices;
            Indices  = indices;
            Center   = center;
            Volume   = volume;
        }
    }
}
