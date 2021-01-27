using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Math
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Color3 : IEquatable<Color3>
    {
        public static readonly Color3 White = new Color3(1, 1, 1);
        public static readonly Color3 Black = new Color3(0, 0, 0);
        public static readonly Color3 Red = new Color3(1, 0, 0);
        public static readonly Color3 Green = new Color3(0, 1, 0);
        public static readonly Color3 Blue = new Color3(0, 0, 1);

        public float R;
        public float G;
        public float B;    

        public Color3(float value)
        {
            this.R = value;
            this.G = value;
            this.B = value;
        }        
            
        public Color3(float red, float green, float blue)
        {         
            this.R = red;
            this.G = green;
            this.B = blue;
        }    


        public static Color3 operator +(Color3 left, Color3 right)
        {
            Color3 color;            
            color.R = left.R + right.R;
            color.G = left.G + right.G;
            color.B = left.B + right.B;
            return color;
        }
        public static Color3 operator *(Color3 color1, Color3 color2)
        {
            Color3 color;            
            color.R = color1.R * color2.R;
            color.G = color1.G * color2.G;
            color.B = color1.B * color2.B;
            return color;
        }
        public static Color3 operator *(Color3 value, float scale)
        {
            Color3 color;
            color.R = value.R * scale;
            color.G = value.G * scale;
            color.B = value.B * scale;
            return color;
        }

        public static Color3 operator *(float scale, Color3 value)
        {
            Color3 color;            
            color.R = value.R * scale;
            color.G = value.G * scale;
            color.B = value.B * scale;
            return color;
        }

        public static Color3 operator -(Color3 left, Color3 right)
        {
            Color3 color;            
            color.R = left.R - right.R;
            color.G = left.G - right.G;
            color.B = left.B - right.B;
            return color;
        }
        public static Color3 operator -(Color3 value)
        {
            Color3 color;            
            color.R = 1.0f - value.R;
            color.G = 1.0f - value.G;
            color.B = 1.0f - value.B;
            return color;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool operator ==(Color3 left, Color3 right)
        {
            return left.R == right.R && left.G == right.G && left.B == right.B;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool operator !=(Color3 left, Color3 right)
        {
            return !(left.R == right.R && left.G == right.G && left.B == right.B);
        }

        public static implicit operator Color3(Vector3 value)
        {
            return new Color3(value.X, value.Y, value.Z);
        }

        public static implicit operator Vector3(Color3 value)
        {
            return new Vector3(value.R, value.G, value.B);
        }
        public static implicit operator Vector4(Color3 value)
        {
            return new Vector4(value.R, value.G, value.B, 1);
        }
        public static implicit operator Color4(Color3 value)
        {
            return new Color4(1, value.R, value.G, value.B);
        }

        public static implicit operator Color3(int value)
        {
            return Color3.FromArgb(value);
        }


        public int ToArgb()
        {
            return (int)((((((((uint)(1 * 255.0)) * 0x100) + ((uint)(this.R * 255.0))) * 0x100) + ((uint)(this.G * 255.0))) * 0x100) + ((uint)(this.B * 255.0)));
        }

        public static Color3 FromArgb(int argb)
        {
            Color3 c;
            float f = 1.0f / 255.0f;
            c.R = f * (float)(byte)(argb >> 16);
            c.G = f * (float)(byte)(argb >> 8);
            c.B = f * (float)(byte)(argb >> 0);
            return c;
        }

        public static Color3 Saturate(Color3 value)
        {
            Color3 v = value;
            v.R = System.Math.Min(1, System.Math.Max(0, value.R));
            v.G = System.Math.Min(1, System.Math.Max(0, value.G));
            v.B = System.Math.Min(1, System.Math.Max(0, value.B));            
            return v;
        }

        public bool Equals(Color3 other)
        {
            return 
                   other.R == R &&
                   other.G == G &&
                   other.B == B;
        }

        public override bool Equals(object obj)
        {
            if (obj is Color3)
                return ((Color3)obj).Equals(this);
            return false;
        }

        public override int GetHashCode()
        {
            return ToArgb();
        }

        public override string ToString()
        {
            return R + ", " + G + ", " + B;
        }

        public List<float> ToList() => new List<float> { R, G, B};
    }
}
