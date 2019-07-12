using System;
using System.Runtime.InteropServices;

namespace VHacdSharp
{
    public partial class VHacd
    {
        internal sealed class Handle : SafeHandle
        {
            public override bool IsInvalid => handle == IntPtr.Zero;

            Handle() : base(IntPtr.Zero, ownsHandle: true) { }

            protected override bool ReleaseHandle()
            {
                VHACD_Release(handle);
                return true;
            }
        }
    }
}
