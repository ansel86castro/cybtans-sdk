using System.Collections.Generic;

namespace Cybtans.Graphics.Collections
{
    public class ReadOnlyList<T> : IReadOnlyList<T>
    {
        internal List<T> Items;

        public ReadOnlyList()
        {
            Items = new List<T>();
        }

        public ReadOnlyList(List<T> items)
        {
            Items = items;
        }

        public T this[int index]
        {
            get { return Items[index]; }
        }

        public int Count
        {
            get { return Items.Count; }
        }

        public List<T>.Enumerator GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}