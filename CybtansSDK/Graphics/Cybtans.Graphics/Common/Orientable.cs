using Cybtans.Math;

namespace Cybtans.Graphics.Common
{
    public static class Orientable
    {
        public static Euler GetEulerAngles(this IRotable o)
        {
            return Euler.FromMatrix(o.LocalRotation);
        }

        public static void SetEulerAngles(this IRotable o, Euler orientation)
        {
            o.LocalRotation = orientation.ToMatrix();
        }
    }
}