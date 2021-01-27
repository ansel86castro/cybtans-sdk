using Cybtans.Graphics.Common;
using System.Collections.Generic;

namespace Cybtans.Graphics.Collections
{
    public class NamedItemCollection<T> : IList<T>
        where T : INameable
    {
        List<T> _items = new List<T>();
        Dictionary<string, int> _lookup = new Dictionary<string, int>();

        public int IndexOf(T item)
        {
            return _lookup[item.Name];
        }

        public void Insert(int index, T item)
        {
            _lookup[item.Name] = index;
            _items.Insert(index, item);
            HookNameChanged(item);
        }

        public void RemoveAt(int index)
        {
            T obj = _items[index];
            _lookup.Remove(obj.Name);
            _items.RemoveAt(index);
            UnHookNameChanged(obj);
        }

        public T this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                var item = _items[index];
                _lookup.Remove(item.Name);
                UnHookNameChanged(item);

                _items[index] = value;
                _lookup[item.Name] = index;
                HookNameChanged(value);
            }
        }

        public T this[string name]
        {
            get
            {
                return _items[_lookup[name]];
            }
        }

        public void Add(T item)
        {
            _lookup[item.Name] = _items.Count;
            _items.Add(item);
            HookNameChanged(item);
        }

        public void Clear()
        {
            foreach (var item in _items)
                UnHookNameChanged(item);

            _items.Clear();
            _lookup.Clear();
        }

        public bool Contains(T item)
        {
            return _lookup.ContainsKey(item.Name);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            int index;
            if (_lookup.TryGetValue(item.Name, out index))
            {
                UnHookNameChanged(item);
                _lookup.Remove(item.Name);
                _items.RemoveAt(index);
                return true;
            }
            return false;
        }

        public List<T>.Enumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        private void HookNameChanged(T item)
        {
            if (item is INameChangingNotificator)
            {
                ((INameChangingNotificator)item).NameChanged += KeyObjectCollection_NameChanged;
            }
        }

        private void UnHookNameChanged(T item)
        {
            if (item is INameChangingNotificator)
            {
                ((INameChangingNotificator)item).NameChanged -= KeyObjectCollection_NameChanged;
            }
        }

        void KeyObjectCollection_NameChanged(object obj, string newName)
        {
            T item = (T)obj;
            int index = _lookup[item.Name];
            _lookup.Remove(item.Name);
            _lookup.Add(newName, index);
        }

    }
}
