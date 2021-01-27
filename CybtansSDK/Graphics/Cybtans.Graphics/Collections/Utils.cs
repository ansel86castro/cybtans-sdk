using System;
using System.Collections.Generic;

namespace Cybtans.Graphics.Collections
{
    public static class Utils
    {
        public static LinkedListNode<T> GetLinkedNode<T>(this LinkedList<T> list, Predicate<T> condition)
        {
            LinkedListNode<T> node = null;
            LinkedListNode<T> current = list.First;
            while (current != null)
            {
                if (condition(current.Value))
                {
                    node = current;
                    break;
                }
                current = current.Next;
            }
            return node;
        }
    }
}
