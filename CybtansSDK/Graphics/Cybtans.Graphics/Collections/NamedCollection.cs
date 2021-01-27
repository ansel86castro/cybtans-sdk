using Cybtans.Graphics.Common;
using System.Collections.ObjectModel;

namespace Cybtans.Graphics.Collections
{
    public class NamedCollection<T> : KeyedCollection<string, T>
        where T : INameable
    {

        protected override string GetKeyForItem(T item)
        {
            return item.Name;
        }
    }
}