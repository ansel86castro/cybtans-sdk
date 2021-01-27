using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Graphics.Common
{
    public static class Generator
    {
        static class GenId<T>
        {
            public static int ObjectId;
        }

        private static int _objectId;

        public static int GenerateId()
        {
            return ++_objectId;
        }

        public static int GenerateId<T>()
        {
            return ++GenId<T>.ObjectId;
        }


        public static string GenerateStringId()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
