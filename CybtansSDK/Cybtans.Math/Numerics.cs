using System;
using System.Collections.Generic;
using System.Text;


namespace Cybtans.Math
{
    public static class Numerics
    {
        public const float PI = (float)System.Math.PI;
        public const float TwoPI = (float)System.Math.PI * 2;
        public const float PIover2 = (float)System.Math.PI / 2;
        public const float PIover3 = (float)System.Math.PI / 3;
        public const float PIover4 = (float)System.Math.PI / 4;
        public const float PIover6 = (float)System.Math.PI / 2;
        public const float E = (float)System.Math.E;
        public const float Epsilon = 5.0e-4f;

        public static float ToRadians(float angle)
        {
            return TwoPI * (angle / 360.0f);
        }

        public static float ToAngle(float radian)
        {
            return 360.0f * (radian / TwoPI);
        }

        public static float Frag(float x)
        {
            return x - (float)System.Math.Truncate(x);
        }

        public static float Saturate(float x)
        {
            if (x >= 1) return 1;
            else if (x <= 0) return 0;
            else return x;
        }

        public static float Clamp(float x, float a, float b)
        {
            if (x >= b) return b;
            else if (x <= a) return a;
            else return x;
        }

        #region Interpolation

        public static Vector3 Lerp(Vector3 v0, Vector3 v1, float x)
        {
            Vector3 r = new Vector3();
            float a = 1 - x;
            r.X = a * v0.X + x * v1.X;
            r.Y = a * v0.Y + x * v1.Y;
            r.Z = a * v0.Z + x * v1.Z;
            return r;
        }

        public static Vector2 Lerp(Vector2 v0, Vector2 v1, float x)
        {
            Vector2 r = new Vector2();
            float a = 1 - x;
            r.X = a * v0.X + x * v1.X;
            r.Y = a * v0.Y + x * v1.Y;
            return r;
        }

        public static Vector4 Lerp(Vector4 v0, Vector4 v1, float x)
        {
            Vector4 r = new Vector4();
            float a = 1 - x;
            r.X = a * v0.X + x * v1.X;
            r.Y = a * v0.Y + x * v1.Y;
            r.Z = a * v0.Z + x * v1.Z;
            r.W = a * v0.W + x * v1.W;
            return r;
        }

        public static unsafe void Lerp(float* r, float* v0, float* v1, float x, int size)
        {           
            for (int i = 0; i < size; i++)
            {
                r[i] = v0[i] + (v1[i] - v0[i]) * x;
            }
        }

        public static float Lerp(float v0, float v1, float x)
        {
            return v0 + (v1 - v0) * x;
        }

        public static float LInterp(float[] xs, float[] ys, float x)
        {
            float l0 = (x - xs[1]) / (xs[0] - xs[1]);
            float l1 = (x - xs[0]) / (xs[1] - xs[0]);
            return l0 * ys[0] + l1 * ys[1];
        }

        public static float CInterp(float[] xs, float[] ys, float x)
        {
            float l0 = ((x - xs[1]) * (x - xs[2])) / ((xs[0] - xs[1]) * (xs[0] - xs[2]));
            float l1 = ((x - xs[0]) * (x - xs[2])) / ((xs[1] - xs[0]) * (xs[1] - xs[2]));
            float l2 = ((x - xs[0]) * (x - xs[1])) / ((xs[2] - xs[0]) * (xs[2] - xs[1]));

            return l0 * ys[0] + l1 * ys[1] + l2 * ys[2];
        }

        public static float PInterp(float[] xs, float[] ys, float x)
        {
            float n = xs.Length;
            float r = 0;

            for (int i = 0; i < n; i++)
            {
                float li = 1;
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                        li *= (x - xs[j]) / (xs[i] - xs[j]);
                }
                r += li * ys[i];
            }

            return r;
        }

        /// <summary>
        /// A cubic Bézier spline equation is given by:
        /// B(s) = p0*(1-s)^3 + 3*c0*s(1-s)^2 + 3*c1*s^2(1-s) + p1*s^3  , s -> [0,1]
        /// </summary>
        /// <returns></returns>
        public static float Bezier(float p0, float p1, float c0, float c1, float s)
        {
            float s2 = s * s;
            float s3 = s2 * s;
            float invs = 1.0f - s;
            float invS2 = invs * invs;
            float invS3 = invS2 * invs;
            return p0 * invS3 + 3 * c0 * s * invS2 + 3 * c1 * s2 * invs + p1 * s3;
        }

        /// <summary>
        /// A cubic Bézier spline equation is given by:
        /// B(s) = p0*(1-s)^3 + 3*c0*s(1-s)^2 + 3*c1*s^2(1-s) + p1*s^3  , s -> [0,1]
        /// </summary>
        /// <returns></returns>
        public static Vector2 Bezier(Vector2 p0, Vector2 p1, Vector2 c0, Vector2 c1, float s)
        {
            float s2 = s * s;
            float s3 = s2 * s;
            float invs = 1.0f - s;
            float invS2 = invs * invs;
            float invS3 = invS2 * invs;
            return p0 * invS3 + 3 * c0 * s * invS2 + 3 * c1 * s2 * invs + p1 * s3;
        }

        /// <summary>
        /// A cubic Bézier spline equation is given by:
        /// B(s) = p0*(1-s)^3 + 3*c0*s(1-s)^2 + 3*c1*s^2(1-s) + p1*s^3  , s -> [0,1]
        /// </summary>
        /// <returns></returns>
        public unsafe static void Bezier(float* p0, float* p1, float *c0, float *c1, float s, float*r ,int lenght)
        {
            float s2 = s * s;
            float s3 = s2 * s;
            float invs = 1.0f - s;
            float invS2 = invs * invs;
            float invS3 = invS2 * invs;

            for (int i = 0; i < lenght; i++)
            {
                r[i] = p0[i] * invS3 + 3 * c0[i] * s * invS2 + 3 * c1[i] * s2 * invs + p1[i] * s3;
            }           
        }

        public unsafe static void Bezier(float* p0, float* p1, float c0, float c1, float s, float* r, int lenght)
        {
            float s2 = s * s;
            float s3 = s2 * s;
            float invs = 1.0f - s;
            float invS2 = invs * invs;
            float invS3 = invS2 * invs;

            for (int i = 0; i < lenght; i++)
            {
                r[i] = p0[i] * invS3 + 3 * c0 * s * invS2 + 3 * c1 * s2 * invs + p1[i] * s3;
            }
        }

        public static float Hermite(float p0, float p1, float t0, float t1, float s)
        {
            float s2 = s * s;
            float s3 = s2 * s;
            return p0 * (2 * s3 - 3 * s2 + 1) + t0 * (s3 - 2 * s2 + s) + p1 * (-2 * s3 + 3 * s2) + t1 * (s3 - s2);
        }

        public static Vector2 Hermite(Vector2 p0, Vector2 p1, Vector2 t0, Vector2 t1, float s)
        {
            float s2 = s * s;
            float s3 = s2 * s;
            return p0 * (2 * s3 - 3 * s2 + 1) + t0 * (s3 - 2 * s2 + s) + p1 * (-2 * s3 + 3 * s2) + t1 * (s3 - s2);
        }


        public unsafe static void Hermite(float* p0, float* p1, float* t0, float* t1, float s, float *r, float length)
        {
            float s2 = s * s;            
            float s3 = s2 * s;

            for (int i = 0; i < length; i++)
            {
                r[i] = p0[i] * (2 * s3 - 3 * s2 + 1) + t0[i] * (s3 - 2 * s2 + s) + p1[i] * (-2 * s3 + 3 * s2) + t1[i] * (s3 - s2);
            }            
        }

        public unsafe static void Hermite(float* p0, float* p1, float t0, float t1, float s, float* r, float length)
        {
            float s2 = s * s;
            float s3 = s2 * s;

            for (int i = 0; i < length; i++)
            {
                r[i] = p0[i] * (2 * s3 - 3 * s2 + 1) + t0 * (s3 - 2 * s2 + s) + p1[i] * (-2 * s3 + 3 * s2) + t1 * (s3 - s2);
            }
        }

        #endregion      

        public static float GaussianDistribution(float x, float y, float stdDeviation)
        {
            float dev2 = stdDeviation * stdDeviation;
            float g = 1.0f / (float)System.Math.Sqrt(TwoPI * dev2);
            return g * (float)System.Math.Exp(-(x * x + y * y) / (2 * dev2));
        }

        public static void GetGaussianOffsets(bool horizontal, Vector2 viewportTexelSize, Vector2[] sampleOffsets, float[] sampleWeights)
        {
            // Get the center texel offset and weight
            sampleWeights[0] = 1.0f * GaussianDistribution(0, 0, 2.0f);
            sampleOffsets[0] = new Vector2(0.0f, 0.0f);
            // Get the offsets and weights for the remaining taps
            if (horizontal)
            {
                for (int i = 1; i < sampleOffsets.Length - 1; i += 2)
                {
                    sampleOffsets[i + 0] = new Vector2(i * viewportTexelSize.X, 0.0f);
                    sampleOffsets[i + 1] = new Vector2(-i * viewportTexelSize.X, 0.0f);
                    sampleWeights[i + 0] = 2.0f * GaussianDistribution((float)i, 0.0f, 3.0f);
                    sampleWeights[i + 1] = 2.0f * GaussianDistribution((float)(i + 1), 0.0f, 3.0f);
                }
            }
            else
            {
                for (int i = 1; i < sampleOffsets.Length - 1; i += 2)
                {
                    sampleOffsets[i + 0] = new Vector2(0.0f, i * viewportTexelSize.Y);
                    sampleOffsets[i + 1] = new Vector2(0.0f, -i * viewportTexelSize.Y);
                    sampleWeights[i + 0] = 2.0f * GaussianDistribution(0.0f, (float)i, 3.0f);
                    sampleWeights[i + 1] = 2.0f * GaussianDistribution(0.0f, (float)(i + 1), 3.0f);
                }
            }
        }
        
        //public static void ComputeEigenVectors(Matrix m, out Vector3 eigenValues, out Vector3[]eigenVectors , float epsilon)
        //{
        //    eigenValues = new Vector3();
        //    eigenVectors = new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1) };
        //    float[,] r = new float[3, 3] { { 1f, 0f, 0f }, { 0f, 1f, 0f }, { 0f, 0f, 1f } };
        //    float u, u2, u2p1, t, c, s,temp;            

        //    for (int i = 0; i < 32; i++)
        //    {
        //        if (System.Math.Abs(m.M12) < epsilon && System.Math.Abs(m.M13) < epsilon && System.Math.Abs(m.M23) < epsilon)
        //            break;

        //        //eliminate [1,2] entry
        //        if (m.M12 != 0)
        //        {
        //             u = (m.M22 - m.M11) * 0.5f / m.M12;
        //             u2 = u * u;
        //             u2p1 = u2 + 1f;
        //             t = (u2 != u2p1) ? ((u < 0 ? -1f : 1f) * (float)System.Math.Sqrt(u2p1) - System.Math.Abs(u)) : 0.5f / u;
        //             c = 1f / (float)System.Math.Sqrt(t * t + 1f);
        //             s = c * t;

        //             m.M11 -= t * m.M12;
        //             m.M22 += t * m.M12;
        //             m.M12 = 0;

        //             temp = c * m.M13 - s * m.M23;
        //             m.M23 = s * m.M13 + c * m.M23;
        //             m.M13 = temp;

        //             for (int j = 0; j < 3; j++)
        //             {
        //                 temp = c * r[j, 0] - s * r[j, 1];
        //                 r[j, 1] = s * r[j, 0] + c * r[j, 1];
        //                 r[j, 0] = temp;
        //             }
        //        }
        //        //eliminate [1,3] entry
        //        if (m.M13 != 0f)
        //        {
        //            u = (m.M33 - m.M11) * 0.5f / m.M13;
        //            u2 = u * u;
        //            u2p1 = u2 + 1f;
        //            t = (u2 != u2p1) ? ((u < 0f ? -1f : 1f) * (float)System.Math.Sqrt(u2p1) - System.Math.Abs(u)) : 0.5f / u;
        //            c = 1f / (float)System.Math.Sqrt(t * t + 1f);
        //            s = c * t;

        //            m.M11 -= t * m.M13;
        //            m.M33 += t * m.M13;
        //            m.M13 = 0;

        //            temp = c * m.M12 - s * m.M23;
        //            m.M23 = s * m.M12 + c * m.M23;
        //            m.M12 = temp;

        //            for (int j = 0; j < 3; j++)
        //            {
        //                temp = c * r[j, 0] - s * r[j, 2];
        //                r[j, 2] = s * r[j, 0] + c * r[j, 2];
        //                r[j, 0] = temp;
        //            }
        //        }

        //        //annihilate (2,3) entry
        //        if (m.M23 != 0f)
        //        {
        //             u = (m.M33 - m.M22) * 0.5f / m.M23;
        //            u2 = u * u;
        //            u2p1 = u2 + 1f;
        //            t = (u2 != u2p1) ? ((u < 0f ? -1f : 1f) * (float)System.Math.Sqrt(u2p1) - System.Math.Abs(u)) : 0.5f / u;
        //            c = 1f / (float)System.Math.Sqrt(t * t + 1f);
        //            s = c * t;

        //            m.M22 -= t * m.M23;
        //            m.M33 += t * m.M23;
        //            m.M23 = 0;

        //            temp = c * m.M12 - s * m.M13;
        //            m.M13 = s * m.M12 + c * m.M13;
        //            m.M12 = temp;

        //            for (int j = 0; j < 3; j++)
        //            {
        //                temp = c * r[j, 1] - s * r[j, 2];
        //                r[j, 2] = s * r[j, 1] + c * r[j, 2];
        //                r[j, 1] = temp;
        //            }
        //        }            
        //    }

        //    eigenValues.X = m.M11;
        //    eigenValues.Y = m.M22;
        //    eigenValues.Z = m.M33;

        //    eigenVectors[0] = new Vector3(r[0, 0], r[1, 0], r[2, 0]);
        //    eigenVectors[1] = new Vector3(r[0, 1], r[1, 1], r[2, 1]);
        //    eigenVectors[2] = new Vector3(r[0, 2], r[1, 2], r[2, 2]);
        //}

        //public static unsafe Matrix ComputeCorrelationMatrix(byte* vertexes, int vertexCount, int stride)
        //{
        //    Vector3 mean = Vector3.Zero;
        //    float n = 1.0f / (float)vertexCount;

        //    Matrix C = new Matrix();
        //    C.M44 = 1;
        //    Vector3* pter;

        //    //Compute Mean
        //    for (int i = 0; i < vertexCount; i++)
        //    {
        //        pter = (Vector3*)(vertexes + i * stride);
        //        mean += *pter;
        //    }
        //    mean *= n;

        //    for (int i = 0; i < vertexCount; i++)
        //    {
        //        pter = (Vector3*)(vertexes + i * stride);

        //        C.M11 += n * (pter->X - mean.X) * (pter->X - mean.X);
        //        C.M12 += n * (pter->X - mean.X) * (pter->Y - mean.Y);
        //        C.M13 += n * (pter->X - mean.X) * (pter->Z - mean.Z);
        //        C.M22 += n * (pter->Y - mean.Y) * (pter->Y - mean.Y);
        //        C.M23 += n * (pter->Y - mean.Y) * (pter->Z - mean.Z);
        //        C.M33 += n * (pter->Z - mean.Z) * (pter->Z - mean.Z);
        //    }
        //    C.M21 = C.M12;
        //    C.M31 = C.M13;
        //    C.M32 = C.M23;
        //    return C;
        //}

        //public static Matrix ComputeCorrelationMatrix(Vector3[] positions)
        //{
        //    Vector3 mean = Vector3.Zero;
        //    float n = 1.0f / (float)positions.Length;
        //    //Compute Mean
        //    for (int i = 0; i < positions.Length; i++)
        //        mean += positions[i];
        //    mean *= n;

        //    Matrix C = new Matrix();
        //    for (int i = 0; i < positions.Length; i++)
        //    {
        //        C.M11 += n * (positions[i].X - mean.X) * (positions[i].X - mean.X);
        //        C.M12 += n * (positions[i].X - mean.X) * (positions[i].Y - mean.Y);
        //        C.M13 += n * (positions[i].X - mean.X) * (positions[i].Z - mean.Z);
        //        C.M22 += n * (positions[i].Y - mean.Y) * (positions[i].Y - mean.Y);
        //        C.M23 += n * (positions[i].Y - mean.Y) * (positions[i].Z - mean.Z);
        //        C.M33 += n * (positions[i].Z - mean.Z) * (positions[i].Z - mean.Z);
        //    }
        //    C.M21 = C.M12;
        //    C.M31 = C.M13;
        //    C.M32 = C.M23;

        //    return C;
        //}

        //public static Matrix ComputeCorrelationMatrix(IEnumerable<Vector3> positions)
        //{
        //    Vector3 mean = Vector3.Zero;
        //    int count = 0;
        //    //Compute Mean
        //    foreach (var p in positions)
        //    {
        //        mean += p;
        //        count++;
        //    }
        //    float n = 1.0f / (float)count;
        //    mean *= n;

        //    Matrix C = new Matrix();
        //    foreach (var p in positions)
        //    {
        //        C.M11 += n * (p.X - mean.X) * (p.X - mean.X);
        //        C.M12 += n * (p.X - mean.X) * (p.Y - mean.Y);
        //        C.M13 += n * (p.X - mean.X) * (p.Z - mean.Z);
        //        C.M22 += n * (p.Y - mean.Y) * (p.Y - mean.Y);
        //        C.M23 += n * (p.Y - mean.Y) * (p.Z - mean.Z);
        //        C.M33 += n * (p.Z - mean.Z) * (p.Z - mean.Z);
        //    }
        //    C.M21 = C.M12;
        //    C.M31 = C.M13;
        //    C.M32 = C.M23;

        //    return C;
        //}

        public static int DoubleFactorial(int n)
        {
            int r = 1;
            for (int i = n; i > 0; i -= 2)
            {
                r *= i;
            }
            return r;
        }       

        public static int Factorial(int n)
        {
            int result = 1;
            for (int i = 1; i <= n; i++)
                result *= i;
            return result;
        }

        public static float DerivateForward1(float[] y, int i, float h)
        {
            return (-3 * y[i] + 4 * y[i + 1] - y[i + 2]) * (0.5f * h);
        }

        public static unsafe float DerivateForward1(float* y, float h)
        {
            return (-3 * y[0] + 4 * y[1] - y[2]) * (0.5f * h);
        }

        public static float DerivateForward1(float y0, float y1, float y2, float h)
        {
            if (h == 0) h = 0.01f;
            return (-3 * y0 + 4 * y1 - y2) / (2 * h);
        }

        public static Vector3 DerivateForward1(Vector3[] y, int i, float h)
        {
            return (-3 * y[i] + 4 * y[i + 1] - y[i + 2]) * (0.5f * h);
        }

        public static unsafe Vector3 DerivateForward1(Vector3* y, float h)
        {
            return (-3 * y[0] + 4 * y[1] - y[2]) * (0.5f * h);
        }

        public static bool IsZero(this float v, float epsilon = Numerics.Epsilon)
        {
            return v == 0 || (v > -epsilon && v < epsilon);
        }

        public static bool IsEqual(this float a, float b, float epsilon = Numerics.Epsilon)
        {
            return a == b || (a - b).IsZero(epsilon);
        }          
    }

    //public class LinearSystem
    //{
    //    float[,] matrix;
    //    float[] r;
    //    int n;
    //    int m;

    //    public float[,] Matrix
    //    {
    //        get { return matrix; }
    //        set
    //        {
    //            matrix = value; 
    //            n = matrix.GetLength(0);
    //            m = matrix.GetLength(1);
    //        }
    //    }
    //    public float[] Constant { get { return r; } set { r = value; } }

    //    public LinearSystem(float[,]matrix , float[]r)
    //    {
    //        this.matrix = matrix;
    //        this.r = r;
    //        n = matrix.GetLength(0);
    //        m = matrix.GetLength(1);
    //    }

    //    public float[] Solve()
    //    {
    //        float[] result = new float[m];

    //        return result;
    //    }
    //}
}
