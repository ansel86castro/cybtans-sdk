using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Math
{

    [Serializable]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct SizeF
    {
        public float Width;
        public float Height;

        public SizeF(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }


        public bool IsEmpty { get { return Width == 0 && Height == 0; } }

        public static readonly SizeF Empty = new SizeF();
    }

    [Serializable]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Size
    {
        public int Width;

        public int Height;

        public Size(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }


        public bool IsEmpty { get { return Width == 0 && Height == 0; } }

        public static readonly Size Empty = new Size();
    }
}
