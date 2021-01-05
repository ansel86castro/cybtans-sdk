using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cybtans.Math
{
    public static class StringConverter
    {
        
        const float TwoPi = (float)System.Math.PI * 2;

        static float ToRadians(float angle)
        {
            return TwoPi * (angle / 360.0f);
        }

        static float ToAngle(float radian)
        {
            return 360.0f * (radian / TwoPi);
        }       

        public static object GetValue(Type type, string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            if (type == typeof(float))
                return float.Parse(value);
            else if (type == typeof(int))
                return int.Parse(value);
            else if (type == typeof(bool))
                return bool.Parse(value);
            else if (type == typeof(short))
                return short.Parse(value);
            else if (type == typeof(double))
                return double.Parse(value);
            else if (type == typeof(byte))
                return byte.Parse(value);
            else if (type == typeof(string))
                return value;
            else if (type == typeof(Vector3))
            {
                //var match = Regex.Match(value, @"(?<X>\d+(\.\d+)?)\s?,\s?(?<Y>\d+(\.\d+)?)\s?,\s?(?<Z>\d+(\.\d+)?)");
                var match = Regex.Match(value, @"\((?<X>-?\d+(\.\d+)?)\s?,\s?(?<Y>-?\d+(\.\d+)?)\s?,\s?(?<Z>-?\d+(\.\d+)?)\)");
                //var match = Regex.Matches(value, @"(-?\d+(\.\d+)?)");
                return new Vector3(float.Parse(match.Groups["X"].Value), float.Parse(match.Groups["Y"].Value), float.Parse(match.Groups["Z"].Value));
            }
            else if (type == typeof(Vector2))
            {
                //var match = Regex.Matches(value, @"(-?\d+(\.\d+)?)");                
                var match = Regex.Match(value, @"\((?<X>-?\d+(\.\d+)?)\s?,\s?(?<Y>-?\d+(\.\d+)?)\)");
                return new Vector2(float.Parse(match.Groups["X"].Value), float.Parse(match.Groups["Y"].Value));
            }
            else if (type == typeof(Vector4))
            {
                //var match = Regex.Matches(value, @"(-?\d+(\.\d+)?)");
                var match = Regex.Match(value, @"\((?<X>-?\d+(\.\d+)?)\s?,\s?(?<Y>-?\d+(\.\d+)?)\s?,\s?(?<Z>-?\d+(\.\d+)?)\s?,\s?(?<W>-?\d+(\.\d+)?)\)");
                return new Vector4(float.Parse(match.Groups["X"].Value), float.Parse(match.Groups["Y"].Value), float.Parse(match.Groups["Z"].Value), float.Parse(match.Groups["W"].Value));
            }
            else if (type == typeof(Euler))
            {
                //var match = Regex.Matches(value, @"(-?\d+(\.\d+)?)");
                var match = Regex.Match(value, @"<(?<X>-?\d+(\.\d+)?)\s?,\s?(?<Y>-?\d+(\.\d+)?)\s?,\s?(?<Z>-?\d+(\.\d+)?)>");
                return new Euler(ToRadians(float.Parse(match.Groups["X"].Value)), ToRadians(float.Parse(match.Groups["Y"].Value)), ToRadians(float.Parse(match.Groups["Z"].Value)));
            }
            else if (type == typeof(SizeF))
            {
                var match = Regex.Match(value, @"<(?<W>-?\d+(\.\d+)?)\s?x\s?(?<H>-?\d+(\.\d+)?)>F");
                //var match = Regex.Matches(value, @"(-?\d+(\.\d+)?)");
                return new SizeF(float.Parse(match.Groups["W"].Value), float.Parse(match.Groups["H"].Value));
            }
            else if (type == typeof(Size))
            {
                var match = Regex.Match(value, @"<(?<W>-?\d+(\.\d+)?)\s?x\s?(?<H>-?\d+(\.\d+)?)>");
                //var match = Regex.Matches(value, @"(-?\d+(\.\d+)?)");
                return new SizeF(float.Parse(match.Groups["W"].Value), float.Parse(match.Groups["H"].Value));
            }
            else if (type == typeof(Spherical))
            {
                var match = Regex.Match(value, @"<(?<THETA>-?\d+(\.\d+)?)\s?,\s?(?<PITCH>-?\d+(\.\d+)?)>");
                //var match = Regex.Matches(value, @"(-?\d+(\.\d+)?)");
                return new SizeF(float.Parse(match.Groups["THETA"].Value), float.Parse(match.Groups["PITCH"].Value));
            }        
            else if (type == typeof(Plane))
            {
                //var match = Regex.Matches(value, @"(-?\d+(\.\d+)?)");
                var match = Regex.Match(value, @"< \((?<X>-?\d+(\.\d+)?)\s?,\s?(?<Y>-?\d+(\.\d+)?)\s?,\s?(?<Z>-?\d+(\.\d+)?)\) \s?,\s? (?<D>-?\d+(\.\d+)?)>", RegexOptions.IgnorePatternWhitespace);
                return new Plane(float.Parse(match.Groups["X"].Value), float.Parse(match.Groups["Y"].Value), float.Parse(match.Groups["Z"].Value), float.Parse(match.Groups["D"].Value));
            }          
            else if (type == typeof(Spherical))
            {
                var match = Regex.Match(value, @"\((?<Theta>-?\d+(\.\d+)?)\s?,\s?(?<Phi>-?\d+(\.\d+)?)\)");
                return new Spherical(ToRadians(float.Parse(match.Groups["Theta"].Value)), ToRadians(float.Parse(match.Groups["Phi"].Value)));
            }
            throw new ArgumentException("Unable to Converto to " + type + " from " + value);
        }

        public static string GetString(object value)
        {
            if (value is Vector3)
            {
                Vector3 v = (Vector3)value;
                return "(" + v.X + " ," + v.Y + " ," + v.Z + ")";
            }
            else if (value is Vector2)
            {
                Vector2 v = (Vector2)value;
                return "(" + v.X + " ," + v.Y + ")";
            }
            else if (value is Vector4)
            {
                Vector4 v = (Vector4)value;
                return "(" + v.X + " ," + v.Y + " ," + v.Z + " ," + v.W + ")";
            }
            else if (value is Plane)
            {
                Plane plane = (Plane)value;
                return String.Format("<{0},{1}>", GetString(plane.Normal), plane.D);
            }
            else if (value is Euler)
            {
                Euler v = (Euler)value;
                return "<" + ToAngle(v.Heading) + " ," + ToAngle(v.Pitch) + " ," + ToAngle(v.Roll) + ">";
            }          
            else if (value is Size)
            {
                Size v = (Size)value;
                return "<" + v.Width + " x " + v.Height + ">";
            }
            else if (value is SizeF)
            {
                SizeF v = (SizeF)value;
                return "<" + v.Width + " x " + v.Height + ">F";
            }
            else if (value is Spherical)
            {
                var s = (Spherical)value;
                return "<" + s.Theta + " x " + s.Phi + ">";
            }

            return value.ToString();
        }
    }
}
