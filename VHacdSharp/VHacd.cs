using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace VHacdSharp
{
    public partial class VHacd : IDisposable
    {
        bool isDisposed;

        GCHandle gcHandle;

        readonly Handle handle;

        public event Action<string> OnLog;

        public event Action<double, double, double, string, string> OnProgressReported;

        public VHacd()
        {
            gcHandle = GCHandle.Alloc(this);
            handle   = VHACD_CreateContext(async: true, GCHandle.ToIntPtr(gcHandle), UserCallback, UserLogger);
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            if (gcHandle.IsAllocated)
                gcHandle.Free();

            VHACD_Cancel(handle);
            handle.Dispose();
            isDisposed = true;
        }

        public async Task<ConvexDecomposition> ComputeAsync(double[] vertices, uint[] triangles, ConvexDecompositionOptions options, CancellationToken cancellationToken = default)
            => await ComputeAsync(new ArraySegment<double>(vertices), new ArraySegment<uint>(triangles), options, cancellationToken);

        public async Task<ConvexDecomposition> ComputeAsync(ArraySegment<double> vertices, ArraySegment<uint> triangles, ConvexDecompositionOptions options, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            // Convert decomposition options to unmanaged representation
            var parameters = UnmanagedComputeParameters.FromDecompositionOptions(options);

            // A proper API for OpenCL support isn't exposed yet,
            // so to be safe it's forced off for now.
            parameters.m_oclAcceleration = 0u;

            VHACD_GetUserPointers(handle,
                out parameters.m_callback,
                out parameters.m_logger
            );

            // The async Compute() method always returns true,
            // so we don't need to bother checking its result.
            VHACD_Compute(handle,
                ref vertices .Array[vertices .Offset], (uint)vertices .Count / 3,
                ref triangles.Array[triangles.Offset], (uint)triangles.Count / 3,
                ref parameters
            );

            // If the token is cancelled, we need to notify the library
            using (cancellationToken.Register(() => VHACD_Cancel(handle)))
            {
                // Unfortunately, the V-HACD API lacks proper completion events,
                // so we need to resort to a busy-loop, checking the IsReady value.
                while (!VHACD_IsReady(handle))
                {
                    ThrowIfDisposed();
                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Yield();
                }
            }

            return new ConvexDecomposition(
                convexHulls:  AllocateConvexHulls(),
                centerOfMass: ComputeCenterOfMass()
            );
        }

        ConvexHull[] AllocateConvexHulls()
        {
            uint count = VHACD_GetNConvexHulls(handle);
            var hulls  = new ConvexHull[count];

            for (uint i = 0; i < count; i++)
            {
                VHACD_GetConvexHull(handle, i, out UnmanagedConvexHull ch);

                var vertices  = new TVector3<double>[ch.m_nPoints];
                var triangles = new uint[ch.m_nTriangles * 3];

                unsafe
                {
                    for (uint j = 0; j < vertices.LongLength; j++)
                    {
                        vertices[j] = new TVector3<double>(
                            ch.m_points[3 * j + 0],
                            ch.m_points[3 * j + 1],
                            ch.m_points[3 * j + 2]
                        );
                    }

                    for (uint j = 0; j < triangles.LongLength; j++)
                        triangles[j] = ch.m_triangles[j];

                    hulls[i] = new ConvexHull(
                        vertices,
                        triangles,
                        ch.m_volume,
                        new TVector3<double>(
                            ch.m_center[0],
                            ch.m_center[1],
                            ch.m_center[2]
                        )
                    );
                }
            }

            return hulls;
        }

        unsafe TVector3<double> ComputeCenterOfMass()
        {
            double* values = stackalloc double[3];
            VHACD_ComputeCenterOfMass(handle, out values[0]);
            return new TVector3<double>(values[0], values[1], values[2]);
        }

        void OnLogCallback(IntPtr message)
            => OnLog?.Invoke(Marshal.PtrToStringAnsi(message));

        void OnProgressCallback(double overallProgress, double stageProgress, double operationProgress, IntPtr stage, IntPtr operation)
        {
            OnProgressReported?.Invoke(
                overallProgress,
                stageProgress,
                operationProgress,
                Marshal.PtrToStringAnsi(stage),
                Marshal.PtrToStringAnsi(operation)
            );
        }

        void ThrowIfDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(VHacd));
        }

        static void UserLogger(IntPtr gcHandle, IntPtr message)
        {
            if (GCHandle.FromIntPtr(gcHandle).Target is VHacd instance)
                instance.OnLogCallback(message);
        }

        static void UserCallback(IntPtr gcHandle, double overallProgress, double stageProgress, double operationProgress, IntPtr stage, IntPtr operation)
        {
            if (GCHandle.FromIntPtr(gcHandle).Target is VHacd instance)
                instance.OnProgressCallback(overallProgress, stageProgress, operationProgress, stage, operation);
        }
    }
}
