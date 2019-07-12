using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace VHacdSharp
{
    public delegate void VHacdLogDelegate(string message);

    public delegate void VHacdProgressDelegate(double overallProgress, double stageProgress, double operationProgress, string stage, string operation);

    public partial class VHacd : IDisposable
    {
        readonly GCHandle gcHandle;

        readonly Handle handle;

        public event VHacdLogDelegate OnLog;

        public event VHacdProgressDelegate OnProgressReported;

        public VHacd()
        {
            gcHandle = GCHandle.Alloc(this);
            handle   = VHACD_CreateContext(async: true, GCHandle.ToIntPtr(gcHandle), UserCallback, UserLogger);
        }

        public void Dispose()
        {
            if (gcHandle.IsAllocated)
                gcHandle.Free();

            handle.Dispose();
        }

        public Task<ConvexDecomposition> ComputeAsync(double[] points, uint[] triangles, ComputeParameters parameters, CancellationToken cancellationToken = default)
        {
            return ComputeAsync(new ArraySegment<double>(points), new ArraySegment<uint>(triangles), parameters, cancellationToken);
        }

        public Task<ConvexDecomposition> ComputeAsync(ArraySegment<double> points, ArraySegment<uint> triangles, ComputeParameters parameters, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                var unmanagedParams = (UnmanagedComputeParameters)parameters;

                // A proper API for OpenCL support isn't exposed yet,
                // so to be safe it's forced off for now.
                unmanagedParams.m_oclAcceleration = 0u;

                VHACD_GetUserPointers(handle,
                    out unmanagedParams.m_callback,
                    out unmanagedParams.m_logger
                );

                // The async Compute() method always returns true,
                // so we don't need to bother checking its result.
                VHACD_Compute(handle,
                    ref points   .Array[points   .Offset], (uint)points   .Count / 3,
                    ref triangles.Array[triangles.Offset], (uint)triangles.Count / 3,
                    ref unmanagedParams
                );

                // If the token is canceled, we need to notify the library
                cancellationToken.Register(() => VHACD_Cancel(handle));

                // Unfortunately, the V-HACD API lacks proper completion events,
                // so we need to resort to a busy-loop, checking the IsReady value.
                while (!VHACD_IsReady(handle))
                    cancellationToken.ThrowIfCancellationRequested();

                return new ConvexDecomposition
                {
                    ConvexHulls  = AllocateConvexHulls(),
                    CenterOfMass = ComputeCenterOfMass()
                };
            });
        }

        public ConvexDecomposition Compute(double[] points, uint[] triangles, ComputeParameters parameters)
        {
            return Compute(new ArraySegment<double>(points), new ArraySegment<uint>(triangles), parameters);
        }

        public ConvexDecomposition Compute(ArraySegment<double> points, ArraySegment<uint> triangles, ComputeParameters parameters)
        {
            var unmanagedParams = (UnmanagedComputeParameters)parameters;

            // A proper API for OpenCL support isn't exposed yet,
            // so to be safe it's forced off for now.
            unmanagedParams.m_oclAcceleration = 0u;

            VHACD_GetUserPointers(handle,
                out unmanagedParams.m_callback,
                out unmanagedParams.m_logger
            );

            // The async Compute() method always returns true,
            // so we don't need to bother checking its result.
            // (we use the async version unconditionally)
            VHACD_Compute(handle,
                ref points   .Array[points   .Offset], (uint)points   .Count / 3,
                ref triangles.Array[triangles.Offset], (uint)triangles.Count / 3,
                ref unmanagedParams
            );

            // Unfortunately, the V-HACD API lacks proper completion events,
            // so we need to resort to a busy-loop, checking the IsReady value.
            while (!VHACD_IsReady(handle)) ;

            return new ConvexDecomposition
            {
                ConvexHulls  = AllocateConvexHulls(),
                CenterOfMass = ComputeCenterOfMass()
            };
        }

        ConvexHull[] AllocateConvexHulls()
        {
            uint count = VHACD_GetNConvexHulls(handle);
            var hulls  = new ConvexHull[count];

            for (uint i = 0; i < count; i++)
            {
                VHACD_GetConvexHull(handle, i, out UnmanagedConvexHull ch);

                var points    = new double[ch.m_nPoints  * 3];
                var triangles = new uint[ch.m_nTriangles * 3];

                unsafe
                {
                    for (uint j = 0; j < points.LongLength; j++)
                        points[j] = ch.m_points[j];

                    for (uint j = 0; j < triangles.LongLength; j++)
                        triangles[j] = ch.m_triangles[j];

                    hulls[i] = new ConvexHull
                    {
                        Points    = points,
                        Triangles = triangles,
                        Volume    = ch.m_volume,
                        Center    = new Vector3D
                        {
                            X = ch.m_center[0],
                            Y = ch.m_center[1],
                            Z = ch.m_center[2]
                        }
                    };
                }
            }

            return hulls;
        }

        unsafe Vector3D ComputeCenterOfMass()
        {
            double* values = stackalloc double[3];
            VHACD_ComputeCenterOfMass(handle, out values[0]);
            return new Vector3D { X = values[0], Y = values[1], Z = values[2] };
        }

        static void UserLogger(IntPtr gcHandle, IntPtr message)
        {
            var vhacd = (VHacd)GCHandle.FromIntPtr(gcHandle).Target;
            vhacd.OnLog?.Invoke(Marshal.PtrToStringAnsi(message));
        }

        static void UserCallback(IntPtr gcHandle, double overallProgress, double stageProgress, double operationProgress, IntPtr stage, IntPtr operation)
        {
            var vhacd = (VHacd)GCHandle.FromIntPtr(gcHandle).Target;
            vhacd.OnProgressReported?.Invoke(
                overallProgress,
                stageProgress,
                operationProgress,
                Marshal.PtrToStringAnsi(stage),
                Marshal.PtrToStringAnsi(operation)
            );
        }
    }
}
