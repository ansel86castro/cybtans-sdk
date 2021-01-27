using System;
using System.Collections.Generic;

namespace Cybtans.Graphics.Collections
{
    public class ObservedCollection<TValue> : ICollection<TValue>
    {
        List<TValue> _items;
        Action<TValue> _itemAdded;
        Action<TValue, int> _itemRemoved;

        public ObservedCollection(Action<TValue> itemAdded, Action<TValue, int> itemRemoved)
        {
            _itemAdded = itemAdded;
            _itemRemoved = itemRemoved;

            _items = new List<TValue>();
        }

        public ObservedCollection(int capacity, Action<TValue> itemAdded, Action<TValue, int> itemRemoved)
        {
            _itemAdded = itemAdded;
            _itemRemoved = itemRemoved;

            _items = new List<TValue>(capacity);
        }

        public TValue this[int index]
        {
            get { return _items[index]; }
        }

        public void Add(TValue item)
        {
            _items.Add(item);
            if (_itemAdded != null)
                _itemAdded(item);
        }

        public void Clear()
        {
            if (_itemRemoved != null)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    _itemRemoved(_items[i], i);
                }
            }
            _items.Clear();
        }

        public bool Contains(TValue item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public int IndexOf(TValue item)
        {
            return _items.IndexOf(item);
        }

        public void RemoveAt(int index)
        {
            var item = _items[index];
            _items.RemoveAt(index);
            if (_itemRemoved != null)
                _itemRemoved(item, index);
        }

        public void AddRange(IEnumerable<TValue> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void RemoveRange(IEnumerable<TValue> items)
        {
            foreach (var item in items)
            {
                Remove(item);
            }
        }

        bool ICollection<TValue>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(TValue item)
        {
            int index = _items.IndexOf(item);
            if (index >= 0)
            {
                _items.RemoveAt(index);
                if (_itemRemoved != null)
                    _itemRemoved(item, index);
                return true;
            }
            return false;
        }

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public List<TValue>.Enumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}