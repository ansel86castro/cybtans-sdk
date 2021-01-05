using System;
using Xunit;

namespace Cybtans.Math.Tests
{
    public class VectorTests
    {
        [Fact]
        public void Vector3_Add()
        {
            var v1 = new Vector3(1, 1, 1);
            var v2 = new Vector3(1, 1, 1);

            var v3 = v1 + v2;

            Assert.Equal(v3, new Vector3(2, 2, 2));
        }

        [Fact]
        public void Vector3_Sub()
        {
            var v1 = new Vector3(1, 1, 1);
            var v2 = new Vector3(1, 1, 1);

            var v3 = v1 - v2;

            Assert.Equal(v3, new Vector3(0, 0, 0));
        }

        [Fact]
        public void Vector3_Mul()
        {
            var v1 = new Vector3(1, 1, 1);
            var v2 = new Vector3(1, 1, 1);

            var v3 = v1 * v2;

            Assert.Equal(v3, new Vector3(1, 1, 1));
        }

        [Fact]
        public void Vector3_Div()
        {
            var v1 = new Vector3(4, 4, 4);
            var v2 = new Vector3(2, 2, 2);

            var v3 = v1 - v2;

            Assert.Equal(v3, new Vector3(2, 2, 2));
        }


        [Fact]
        public void Vector3_Dot()
        {                  
            Assert.Equal(0, Vector3.Dot(new Vector3(1, 0, 0), new Vector3(0, 1, 0)));
            
            Assert.Equal(1, Vector3.Dot(new Vector3(1, 0, 0), new Vector3(1, 0, 0)));
        }
    }
}
