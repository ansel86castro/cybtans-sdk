using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Graphics.Collections
{
    public class ResourceCollecion<T> : IReadOnlyCollection<T>
    {
        T[] _resources;

        public ResourceCollecion(T[] resources)
        {
            _resources = resources;
        }

        public int Count { get { return _resources.Length; } }

        public T this[int index]
        {
            get { return _resources[index]; }
        }

        public void CopyTo(int startIndex, int elements, T[] array)
        {
            Array.Copy(_resources, startIndex, array, 0, elements);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _resources.Cast<T>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }
    }
}
