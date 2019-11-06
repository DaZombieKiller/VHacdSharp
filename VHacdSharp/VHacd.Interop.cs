using System;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.CallingConvention;

namespace VHacdSharp
{
    partial class VHacd
    {
        const string DllName = "libVHACD";

        [UnmanagedFunctionPointer(Cdecl)]
        delegate void UserLoggerDelegate(IntPtr gcHandle, IntPtr message);

        [UnmanagedFunctionPointer(Cdecl)]
        delegate void UserCallbackDelegate(IntPtr gcHandle, double overallProgress, double stageProgress, double operationProgress, IntPtr stage, IntPtr operation);

        [DllImport(DllName, CallingConvention = Cdecl)]
        static extern Handle VHACD_CreateContext([MarshalAs(UnmanagedType.I1)] bool async, IntPtr gcHandle, UserCallbackDelegate callback, UserLoggerDelegate logger);

        [DllImport(DllName, CallingConvention = Cdecl)]
        static extern void VHACD_GetUserPointers(Handle self, out IntPtr callback, out IntPtr logger);

        [DllImport(DllName, CallingConvention = Cdecl)]
        static extern void VHACD_GetVersion(out int major, out int minor);

        [DllImport(DllName, CallingConvention = Cdecl)]
        static extern void VHACD_Cancel(Handle self);

        [DllImport(DllName, CallingConvention = Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool VHACD_Compute(Handle self, in double points, uint countPoints, in uint triangles, uint countTriangles, in ComputeParameters parameters);

        [DllImport(DllName, CallingConvention = Cdecl)]
        static extern uint VHACD_GetNConvexHulls(Handle self);

        [DllImport(DllName, CallingConvention = Cdecl)]
        static extern void VHACD_GetConvexHull(Handle self, uint index, out UnmanagedConvexHull ch);

        [DllImport(DllName, CallingConvention = Cdecl)]
        static extern void VHACD_Release(IntPtr self);

        [DllImport(DllName, CallingConvention = Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool VHACD_OCLInit(Handle self, IntPtr oclDevice);

        [DllImport(DllName, CallingConvention = Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool VHACD_OCLRelease(Handle self);

        [DllImport(DllName, CallingConvention = Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool VHACD_ComputeCenterOfMass(Handle self, out double centerOfMass);

        [DllImport(DllName, CallingConvention = Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool VHACD_IsReady(Handle self);

        unsafe struct UnmanagedConvexHull
        {
            public double* m_points;
            public uint* m_triangles;
            public uint m_nPoints;
            public uint m_nTriangles;
            public double m_volume;
            public fixed double m_center[3];
        }

        struct ComputeParameters
        {
            public double m_concavity;
            public double m_alpha;
            public double m_beta;
            public double m_minVolumePerCH;
            public IntPtr m_callback;
            public IntPtr m_logger;
            public uint m_resolution;
            public uint m_maxNumVerticesPerCH;
            public uint m_planeDownsampling;
            public uint m_convexhullDownsampling;
            public uint m_pca;
            public uint m_mode;
            public uint m_convexhullApproximation;
            public uint m_oclAcceleration;
            public uint m_maxConvexHulls;
            public bool m_projectHullVertices;

            public static ComputeParameters FromDecompositionOptions(in ConvexDecompositionOptions options) => new ComputeParameters
            {
                m_resolution              = options.Resolution,
                m_concavity               = options.Concavity,
                m_planeDownsampling       = options.PlaneDownsampling,
                m_convexhullDownsampling  = options.HullDownsampling,
                m_alpha                   = options.Alpha,
                m_beta                    = options.Beta,
                m_pca                     = options.PrincipalComponentAnalysis ? 1u : 0u,
                m_mode                    = (uint)options.DecompositionMode,
                m_maxNumVerticesPerCH     = options.MaxHullVertices,
                m_minVolumePerCH          = options.MinHullVolume,
                m_convexhullApproximation = options.HullApproximation ? 1u : 0u,
                m_oclAcceleration         = options.OpenCLAcceleration ? 1u : 0u,
                m_maxConvexHulls          = options.MaxHulls,
                m_projectHullVertices     = options.HullVertexProjection
            };
        }
    }
}
