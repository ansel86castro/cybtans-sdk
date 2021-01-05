using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Math
{
   
    public readonly struct Derivable32
    {
        public Derivable32(float value)
        {
            Value = value;
            Derivate = 0;
        }

        public Derivable32(float value, float derivate)
        {
            Value = value;
            Derivate = derivate;
        }

        public readonly float Value;

        public readonly float Derivate;

        public static Derivable32 Sum(Derivable32 left, Derivable32 right)
            => new Derivable32(left.Value + right.Value, left.Derivate + right.Derivate);

        public static Derivable32 Sub(Derivable32 left, Derivable32 right)
          => new Derivable32(left.Value - right.Value, left.Derivate - right.Derivate);

        public static Derivable32 Mul(Derivable32 a, Derivable32 b)
          => new Derivable32(a.Value * b.Value,  a.Derivate * b.Value + a.Value*b.Derivate);

        public static Derivable32 Div(Derivable32 a, Derivable32 b)
          => new Derivable32(a.Value / b.Value, (a.Derivate * b.Value - a.Value * b.Derivate) / (b.Value * b.Value));


    }
}
