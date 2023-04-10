using System;
using System.Collections;
using System.Collections.Generic;

namespace Padoru.Core
{
    public enum ListEvent
    {
        CollectionChanged = 0,
        ElementAdded = 1,
        ElementRemoved = 2,
        ElementChanged = 3,
    }
    
    [Serializable]
    public class SubscribableListValue<T> : IList<T>
    {
        private List<T> innerList = new();
        
        public int Count => innerList.Count;
        public bool IsReadOnly => false;

        [field: NonSerialized]
        private event Action<ListEvent, T> OnListChanged;
        
        public T this[int index]
        {
            get
            {
                return innerList[index];
            }
            set
            {
                innerList[index] = value;
                OnListChanged?.Invoke(ListEvent.ElementChanged, value);
            }
        }

        /// <summary>
        /// Subscribe to list changes and return the unsubscribe action.
        /// </summary>
        /// <param name="subscriber"></param>
        /// <returns></returns>
        public Action Subscribe(Action<ListEvent, T> subscriber)
        {
            OnListChanged += subscriber;
            return () => OnListChanged -= subscriber;
        }
        
        /// <summary>
        /// Unsubscribe from list changes
        /// </summary>
        /// <param name="subscriber"></param>
        public void Unsubscribe(Action<ListEvent, T> subscriber)
        {
            OnListChanged -= subscriber;
        }
        
        public void Add(T item)
        {
            innerList.Add(item);
            OnListChanged?.Invoke(ListEvent.ElementAdded, item);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            innerList.AddRange(collection);
            OnListChanged?.Invoke(ListEvent.CollectionChanged, default);
        }

        public void Clear()
        {
            innerList.Clear();
            OnListChanged?.Invoke(ListEvent.CollectionChanged, default);
        }

        public bool Contains(T item)
        {
            return innerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            innerList.CopyTo(array, arrayIndex);
        }

        public void Overwrite(IEnumerable<T> elements)
        {
            innerList.Clear();
            innerList.AddRange(elements);
            OnListChanged?.Invoke(ListEvent.CollectionChanged, default);
        }

        public bool Exists(Predicate<T> match)
        {
            return innerList.Exists(match);
        }
        
        public bool Remove(T item)
        {
            var wasRemoved = innerList.Remove(item);

            if (wasRemoved)
            {
                OnListChanged?.Invoke(ListEvent.ElementRemoved, item);
            }

            return wasRemoved;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return innerList.IndexOf(item);
        }

        public int IndexOf(T item, int index)
        {
            return innerList.IndexOf(item, index);
        }

        public int IndexOf(T item, int index, int count)
        {
            return innerList.IndexOf(item, index, count);
        }

        public T FindLast(Predicate<T> match)
        {
            return innerList.FindLast(match);
        }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            return innerList.FindLastIndex(startIndex, count, match);
        }

        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            return innerList.FindLastIndex(startIndex, match);
        }

        public int FindLastIndex(Predicate<T> match)
        {
            return innerList.FindLastIndex(match);
        }

        public void ForEach(Action<T> action)
        {
            innerList.ForEach(action);
        }

        public bool TrueForAll(Predicate<T> match)
        {
            return innerList.TrueForAll(match);
        }

        public void Reverse()
        {
            innerList.Reverse();
            OnListChanged?.Invoke(ListEvent.CollectionChanged, default);        
        }

        public void Reverse(int index, int count)
        {
            innerList.Reverse(index, count);
            OnListChanged?.Invoke(ListEvent.CollectionChanged, default);
        }

        public void Sort()
        {
            innerList.Sort();
            OnListChanged?.Invoke(ListEvent.CollectionChanged, default);
        }

        public void Sort(IComparer<T> comparer)
        {
            innerList.Sort(comparer);
            OnListChanged?.Invoke(ListEvent.CollectionChanged, default);
        }

        public void Sort(Comparison<T> comparison)
        {
            innerList.Sort(comparison);
            OnListChanged?.Invoke(ListEvent.CollectionChanged, default);
        }

        public void Sort(int index, int count, IComparer<T> comparer)
        {
            innerList.Sort(index, count, comparer);
            OnListChanged?.Invoke(ListEvent.CollectionChanged, default);
        }
        
        public void Insert(int index, T item)
        {
            innerList.Insert(index, item);
            OnListChanged?.Invoke(ListEvent.ElementAdded, item);
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            innerList.InsertRange(index, collection);
            OnListChanged?.Invoke(ListEvent.CollectionChanged, default);
        }

        public void RemoveAt(int index)
        {
            var item = innerList[index];
            innerList.RemoveAt(index);
            OnListChanged?.Invoke(ListEvent.ElementRemoved, item);
        }
        
        public void RemoveRange(int index, int count)
        {
            innerList.RemoveRange(index, count);
            OnListChanged?.Invoke(ListEvent.CollectionChanged, default);
        }
    }
}