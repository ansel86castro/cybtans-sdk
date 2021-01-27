using System.Collections.Generic;

namespace Cybtans.Graphics.Collections
{
    public class ReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        internal List<TValue> Items = new List<TValue>();
        internal Dictionary<TKey, TValue> ItemLookup = new Dictionary<TKey, TValue>();

        public bool ContainsKey(TKey key)
        {
            return ItemLookup.ContainsKey(key);
        }

        public IEnumerable<TKey> Keys
        {
            get { return ItemLookup.Keys; }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return ItemLookup.TryGetValue(key, out value);
        }

        public IEnumerable<TValue> Values
        {
            get { return ItemLookup.Values; }
        }

        public TValue this[TKey key]
        {
            get { return ItemLookup[key]; }
        }

        public TValue this[int index]
        {
            get { return Items[index]; }
        }

        public int Count
        {
            get { return Items.Count; }
        }

        public List<TValue>.Enumerator GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return ItemLookup.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ItemLookup.GetEnumerator();
        }

        internal void Add(TKey key, TValue item)
        {
            ItemLookup.Add(key, item);
            Items.Add(item);
        }

        internal bool Remove(TKey key)
        {
            if (ItemLookup.ContainsKey(key))
            {
                TValue v = ItemLookup[key];
                ItemLookup.Remove(key);
                Items.Remove(v);
                return true;
            }
            return false;
        }
    }
}