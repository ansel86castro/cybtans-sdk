using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Math
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct Ray
    {
        public Vector3 Position;
        public Vector3 Direction;

    }
}
