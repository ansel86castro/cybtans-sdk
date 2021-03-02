using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Math
{   
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Color4:IEquatable<Color4>
    {
        public static readonly Color4 White = new Color4(1, 1, 1, 1);
        public static readonly Color4 Black = new Color4(1, 0, 0, 0);
        public static readonly Color4 Red   = new Color4(1, 1, 0, 0);
        public static readonly Color4 Green = new Color4(1, 0, 1, 0);
        public static readonly Color4 Blue  = new Color4(1, 0, 0, 1);

        public float R;
        public float G;
        public float B;
        public float A;
        
        public Color4(int argb)
        {
            float f = 1.0f / 255.0f;
            R = f * (float)(byte)(argb >> 16);
            G = f * (float)(byte)(argb >> 8);
            B = f * (float)(byte)(argb >> 0);
            A = f * (float)(byte)(argb >> 24);
        }
     
        public Color4(float red, float green, float blue)
        {
            this.A = 1f;
            this.R = red;
            this.G = green;
            this.B = blue;
        }

        public Color4(Color3 color, float alpha)
        {
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
            this.A = alpha;
        }

        public Color4(Color3 color)
        {
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
            this.A = 1;
        }

        public Color4(float alpha, float red, float green, float blue)
        {
            this.A = alpha;
            this.R = red;
            this.G = green;
            this.B = blue;
        }
             

        public static Color4 operator +(Color4 left, Color4 right)
        {
            Color4 color;
            color.A = left.A + right.A;
            color.R = left.R + right.R;
            color.G = left.G + right.G;
            color.B = left.B + right.B;
            return color;
        }
        public static Color4 operator *(Color4 color1, Color4 color2)
        {
            Color4 color;
            color.A = color1.A * color2.A;
            color.R = color1.R * color2.R;
            color.G = color1.G * color2.G;
            color.B = color1.B * color2.B;
            return color;
        }
        public static Color4 operator *(Color4 value, float scale)
        {
            Color4 color;
            color.A = value.A;
            color.R = value.R * scale;
            color.G = value.G * scale;
            color.B = value.B * scale;
            return color;
        }

        public static Color4 operator *(float scale , Color4 value)
        {
            Color4 color;
            color.A = value.A;
            color.R = value.R * scale;
            color.G = value.G * scale;
            color.B = value.B * scale;
            return color;
        }

        public static Color4 operator -(Color4 left, Color4 right)
        {
            Color4 color;
            color.A = left.A - right.A;
            color.R = left.R - right.R;
            color.G = left.G - right.G;
            color.B = left.B - right.B;
            return color;
        }
        public static Color4 operator -(Color4 value)
        {
            Color4 color;
            color.A = 1.0f - value.A;
            color.R = 1.0f - value.R;
            color.G = 1.0f - value.G;
            color.B = 1.0f - value.B;
            return color;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool operator ==(Color4 left, Color4 right)
        {
            return left.A == right.A && left.R == right.R && left.G == right.G && left.B == right.B;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool operator !=(Color4 left, Color4 right)
        {
            return !(left.A == right.A && left.R == right.R && left.G == right.G && left.B == right.B);
        }

        public static explicit operator Vector3(Color4 value)
        {
            return new Vector3(value.R, value.G, value.B);
        }

        public static explicit operator Color3(Color4 value)
        {
            return new Color3(value.R, value.G, value.B);
        }

        public static implicit operator Vector4(Color4 value)
        {
            return new Vector4(value.R, value.G, value.B, value.A);
        }
      
        public static explicit operator Color4(int value)
        {
            return new Color4(value);
        }

        public static Color4 FromArgb(int color)
        {
            return new Color4(color);
        }

        public static Color4 FromRgba(int rgba)
        {
            Color4 c;
            float f = 1.0f / 255.0f;
            c.R = f * (float)(byte)(rgba >> 24);
            c.G = f * (float)(byte)(rgba >> 16);
            c.B = f * (float)(byte)(rgba >> 8);
            c.A = f * (float)(byte)(rgba >> 0);
            return c;
        }

        public int ToRgba()
        {
            return        (int)((((((((uint)(this.R * 255.0)) * 0x100) + 
                                     ((uint)(this.G * 255.0))) * 0x100) + 
                                     ((uint)(this.B * 255.0))) * 0x100) + 
                                     ((uint)(this.A * 255.0)));
        }
        
        public int ToArgb()
        {
            return (int)((((((((uint)(this.A * 255.0)) * 0x100) + ((uint)(this.R * 255.0))) * 0x100) + ((uint)(this.G * 255.0))) * 0x100) + ((uint)(this.B * 255.0)));
        }

        public static int ConvertToRGBA(int abgr)
        {
            byte r = (byte)abgr;
            byte g = (byte)(abgr >> 8);
            byte b = (byte)(abgr >> 16);
            byte a = (byte)(abgr >> 24);

            var result =  ((int)r << 24) |
                    ((int)g << 16) |
                    ((int)b << 8) |
                    ((int)a);
            return result;
        }

        public static Color4 Saturate(Color4 value)
        {
            Color4 v = value;
            v.R = System.Math.Min(1, System.Math.Max(0, value.R));
            v.G = System.Math.Min(1, System.Math.Max(0, value.G));
            v.B = System.Math.Min(1, System.Math.Max(0, value.B));
            v.A = System.Math.Min(1, System.Math.Max(0, value.A));
            return v;
        }

        public bool Equals(Color4 other)
        {
            return other.A == A &&
                   other.R == R &&
                   other.G == G &&
                   other.B == B;
        }

        public override bool Equals(object obj)
        {
            if (obj is Color4)
                return (Color4)obj == this;
            return false;
        }

        public override int GetHashCode()
        {
            return ToArgb();
        }

        public override string ToString()
        {
            return R + ", " + G + ", " + B + ", " + A;
        }

        public List<float> ToList() => new List<float> { R, G, B, A };
    }

   
}
