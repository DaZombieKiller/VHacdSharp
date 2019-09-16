using System;

namespace VHacdSharp
{
    public struct ConvexDecompositionOptions
    {
        /// <summary>Maximum concavity.</summary>
        public double Concavity { get; private set; }

        /// <summary>Bias toward clipping along symmetry planes.</summary>
        public double Alpha { get; private set; }

        /// <summary>Bias toward clipping along revolution axes.</summary>
        public double Beta { get; private set; }

        /// <summary>Minimum volume to add vertices to convex-hulls.</summary>
        public double MinHullVolume { get; private set; }

        /// <summary>Maximum number of voxels generated during the voxelization stage.</summary>
        public uint Resolution { get; private set; }

        /// <summary>Maximum number of vertices per convex-hull.</summary>
        public uint MaxHullVertices { get; private set; }

        /// <summary>Granularity of the search for the "best" clipping plane.</summary>
        public uint PlaneDownsampling { get; private set; }

        /// <summary>Precision of the convex-hull generation process during the clipping plane selection stage.</summary>
        public uint HullDownsampling { get; private set; }

        /// <summary>Enable/disable normalizing the mesh before applying the convex decomposition.</summary>
        public bool PrincipalComponentAnalysis { get; private set; }

        /// <summary>Approximate convex decomposition mode.</summary>
        public DecompositionMode DecompositionMode { get; private set; }

        public bool HullApproximation { get; private set; }

        public bool OpenCLAcceleration { get; private set; }

        /// <summary>Maximum number of convex hulls to produce from the merge operation.</summary>
        public uint MaxHulls { get; private set; }

        /// <summary>Project the output convex hull vertices onto the original source mesh to increase the floating point accuracy of the results.</summary>
        public bool ProjectHullVertices { get; private set; }

        public static readonly ConvexDecompositionOptions Default = new ConvexDecompositionOptions()
            .WithResolution(100_000)
            .WithConcavity(.001)
            .WithPlaneDownsampling(4)
            .WithHullDownsampling(4)
            .WithAlpha(.05)
            .WithBeta(.05)
            .WithDecompositionMode(DecompositionMode.Voxel)
            .WithMaxHullVertices(64)
            .WithMinHullVolume(.0001)
            .WithHullApproximation()
            .WithOpenCLAcceleration()
            .WithMaxHulls(1024)
            .WithProjectHullVertices();

        ConvexDecompositionOptions(ConvexDecompositionOptions source)
            => this = source;

        public ConvexDecompositionOptions WithConcavity(double concavity)
        {
            if (concavity < 0 || concavity > 1)
                throw new ArgumentOutOfRangeException(nameof(concavity));

            return new ConvexDecompositionOptions(this) { Concavity = concavity };
        }

        public ConvexDecompositionOptions WithAlpha(double alpha)
        {
            if (alpha < 0 || alpha > 1)
                throw new ArgumentOutOfRangeException(nameof(alpha));

            return new ConvexDecompositionOptions(this) { Alpha = alpha };
        }

        public ConvexDecompositionOptions WithBeta(double beta)
        {
            if (beta < 0 || beta > 1)
                throw new ArgumentOutOfRangeException(nameof(beta));

            return new ConvexDecompositionOptions(this) { Beta = beta };
        }

        public ConvexDecompositionOptions WithMinHullVolume(double minHullVolume)
        {
            if (minHullVolume < 0 || minHullVolume > .01)
                throw new ArgumentOutOfRangeException(nameof(minHullVolume));

            return new ConvexDecompositionOptions(this) { MinHullVolume = minHullVolume };
        }

        public ConvexDecompositionOptions WithResolution(uint resolution)
        {
            if (resolution < 10_000 || resolution > 64_000_000)
                throw new ArgumentOutOfRangeException(nameof(resolution));

            return new ConvexDecompositionOptions(this) { Resolution = resolution };
        }

        public ConvexDecompositionOptions WithMaxHullVertices(uint maxHullVertices)
        {
            if (maxHullVertices < 4 || maxHullVertices > 1024)
                throw new ArgumentOutOfRangeException(nameof(maxHullVertices));

            return new ConvexDecompositionOptions(this) { MaxHullVertices = maxHullVertices };
        }

        public ConvexDecompositionOptions WithPlaneDownsampling(uint planeDownsampling)
        {
            if (planeDownsampling < 1 || planeDownsampling > 16)
                throw new ArgumentOutOfRangeException(nameof(planeDownsampling));

            return new ConvexDecompositionOptions(this) { PlaneDownsampling = planeDownsampling };
        }

        public ConvexDecompositionOptions WithHullDownsampling(uint hullDownsampling)
        {
            if (hullDownsampling < 1 || hullDownsampling > 16)
                throw new ArgumentOutOfRangeException(nameof(hullDownsampling));

            return new ConvexDecompositionOptions(this) { HullDownsampling = hullDownsampling };
        }

        public ConvexDecompositionOptions WithPrincipalComponentAnalysis(bool enable = true)
            => new ConvexDecompositionOptions(this) { PrincipalComponentAnalysis = enable };

        public ConvexDecompositionOptions WithDecompositionMode(DecompositionMode mode)
            => new ConvexDecompositionOptions(this) { DecompositionMode = mode };

        public ConvexDecompositionOptions WithHullApproximation(bool enable = true)
            => new ConvexDecompositionOptions(this) { HullApproximation = enable };

        public ConvexDecompositionOptions WithOpenCLAcceleration(bool enable = true)
            => new ConvexDecompositionOptions(this) { OpenCLAcceleration = enable };

        public ConvexDecompositionOptions WithMaxHulls(uint maxHulls)
            => new ConvexDecompositionOptions(this) { MaxHulls = maxHulls };

        public ConvexDecompositionOptions WithProjectHullVertices(bool enable = true)
            => new ConvexDecompositionOptions(this) { ProjectHullVertices = enable };
    }
}
