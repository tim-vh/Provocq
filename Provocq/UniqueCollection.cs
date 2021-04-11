using System;
using System.Collections;
using System.Collections.Generic;

namespace Provocq
{
    public class UniqueCollection<T> : ICollection<T>
    {
        private readonly HashSet<T> _hashSet = new HashSet<T>();

        public int Count => _hashSet.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (!_hashSet.Add(item))
            {
                throw new ArgumentException("An item with the same value has already been added.");
            }
        }

        public void Clear()
        {
            _hashSet.Clear();
        }

        public bool Contains(T item)
        {
            return _hashSet.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _hashSet.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _hashSet.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return _hashSet.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _hashSet.GetEnumerator();
        }
    }
}
