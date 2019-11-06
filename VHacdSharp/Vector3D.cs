using System;

namespace VHacdSharp
{
    public struct Vector3D
    {
        public double X, Y, Z;

        public Vector3D(double x, double y, double z) => (X, Y, Z) = (x, y, z);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public override string ToString() => $"({X}, {Y}, {Z})";
    }
}
