using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace Cybtans.Math
{
    
    public class Rand
    {
        static Random _r = new Random();

        public static float Uniform01()
        {
            return (float)_r.NextDouble();
        }

        public static float Uniform(float a, float b)
        {
            return a + (float)_r.NextDouble() * (b - a);
        }

        public static Vector3 RandomUnitVector()
        {
            return Vector3.Normalize(new Vector3(Uniform(-1, 1), Uniform(-1, 1), Uniform(-1, 1)));
        }
    }              

    
}
