using System;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.CallingConvention;

namespace VHacdSharp
{
    partial class VHacd
    {
        const string DllName = "libvhacd";

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
        static extern bool VHACD_Compute(Handle self, ref double points, uint countPoints, ref uint triangles, uint countTriangles, ref UnmanagedComputeParameters parameters);

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

        struct UnmanagedComputeParameters
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

            public static explicit operator UnmanagedComputeParameters(ComputeParameters parameters)
            {
                return new UnmanagedComputeParameters
                {
                    m_resolution              = parameters.Resolution,
                    m_concavity               = parameters.Concavity,
                    m_planeDownsampling       = parameters.PlaneDownsampling,
                    m_convexhullDownsampling  = parameters.HullDownsampling,
                    m_alpha                   = parameters.Alpha,
                    m_beta                    = parameters.Beta,
                    m_pca                     = parameters.PCA ? 1u : 0u,
                    m_mode                    = (uint)parameters.DecompositionMode,
                    m_maxNumVerticesPerCH     = parameters.MaxHullVertices,
                    m_minVolumePerCH          = parameters.MinHullVolume,
                    m_convexhullApproximation = parameters.HullApproximation ? 1u : 0u,
                    m_oclAcceleration         = parameters.OpenCLAcceleration ? 1u : 0u,
                    m_maxConvexHulls          = parameters.MaxHulls,
                    m_projectHullVertices     = parameters.ProjectHullVertices
                };
            }
        }
    }
}
