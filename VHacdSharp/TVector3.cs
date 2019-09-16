using System;

namespace VHacdSharp
{
    public struct TVector3<T>
        where T : unmanaged, IConvertible
    {
        public T X, Y, Z;

        public TVector3(T x, T y, T z)
            => (X, Y, Z) = (x, y, z);

        public override int GetHashCode()
            => HashCode.Combine(X, Y, Z);

        public override string ToString()
            => $"({X}, {Y}, {Z})";

        public string ToString(IFormatProvider provider)
            => $"({X.ToString(provider)}, {Y.ToString(provider)}, {Z.ToString(provider)})";
    }

    public static class TVectorExtensions
    {
#if UNITY_STANDALONE
        public static Vector3 ToUnity(this TVector3<float> vector) => new Vector3
        {
            x = vector.X,
            y = vector.Y,
            z = vector.Z
        };

        public static Vector3Int ToUnity(this TVector3<int> vector) => new Vector3Int
        {
            x = vector.X,
            y = vector.Y,
            z = vector.Z
        };
#endif
    }
}
