using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Graphics.Collections
{
    public class ObservedDictionary<TKey, TValue> : ICollection<TValue>
    {
        List<TValue> _items;
        Dictionary<TKey, TValue> _itemLookup;

        Action<TValue> _itemAdded;
        Action<TValue> _itemRemoved;
        Func<TValue, TKey> _keySelector;

        public ObservedDictionary(Action<TValue> itemAdded, Action<TValue> itemRemoved, Func<TValue, TKey> keySelector)
        {
            _itemAdded = itemAdded;
            _itemRemoved = itemRemoved;
            _keySelector = keySelector;

            _items = new List<TValue>();
            _itemLookup = new Dictionary<TKey, TValue>();
        }

        public ObservedDictionary(int capacity, Action<TValue> itemAdded, Action<TValue> itemRemoved, Func<TValue, TKey> keySelector)
        {
            _itemAdded = itemAdded;
            _itemRemoved = itemRemoved;
            _keySelector = keySelector;

            _items = new List<TValue>(capacity);
            _itemLookup = new Dictionary<TKey, TValue>(capacity);
        }

        public TValue this[int index]
        {
            get { return _items[index]; }
        }

        public TValue this[TKey key]
        {
            get { return _itemLookup[key]; }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _itemLookup.TryGetValue(key, out value);
        }

        public void Add(TValue item)
        {
            var key = _keySelector(item);
            if (_itemLookup.ContainsKey(key))
                return;

            _items.Add(item);
            _itemLookup.Add(key, item);
            if (_itemAdded != null)
                _itemAdded(item);
        }

        public void Clear()
        {
            _items.Clear();
            _itemLookup.Clear();
        }

        public bool Contains(TValue item)
        {
            return _itemLookup.ContainsKey(_keySelector(item));
        }

        public bool ContainsKey(TKey key)
        {
            return _itemLookup.ContainsKey(key);
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
            var key = _keySelector(item);
            _itemLookup.Remove(key);

            if (_itemRemoved != null)
                _itemRemoved(item);
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
            var key = _keySelector(item);
            if (_itemLookup.Remove(key))
            {
                _items.Remove(item);
                if (_itemRemoved != null)
                    _itemRemoved(item);
                return true;
            }
            return false;
        }

        public void ChangeKey(TKey oldKey, TKey newKey)
        {
            var item = _itemLookup[oldKey];
            _itemLookup.Remove(oldKey);
            _itemLookup.Add(newKey, item);
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

        public override string ToString()
        {

            return string.Join(", ", _items);
        }
    }
}
