using System;
using System.Collections.Generic;

namespace Padoru.Core
{
    [Serializable]
    public class SubscribableListValue<T>
    {
        private readonly List<T> value = new();
        public IReadOnlyList<T> Value => value;
        
        public int Count => value.Count;
        
        private event Action<IReadOnlyList<T>> OnValueChanged;
        
        /// <summary>
        /// Add an element to the list.
        /// </summary>
         public void Add(T element)
        {
            value.Add(element);
            OnValueChanged?.Invoke(Value);
        }

        /// <summary>
        /// Return true if the element is contained in the list.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool Contains(T element)
        {
            return value.Contains(element);
        }
        
        /// <summary>
        /// Add a list of elements to the list.
        /// </summary>
        public void AddRange(IEnumerable<T> element)
        {
            value.AddRange(element);
            OnValueChanged?.Invoke(Value);
        }

        /// <summary>
        /// Remove an element from the list.
        /// </summary>
        public void Remove(T element)
        {
            value.Remove(element);
            OnValueChanged?.Invoke(Value);
        }

        /// <summary>
        /// Remove a count of elements starting at the target index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public void RemoveRange(int index, int count)
        {
            value.RemoveRange(index, count);
        }

        /// <summary>
        /// Remove an element from the list at the given index.
        /// </summary>
        public void RemoveAt(int index)
        {
            value.RemoveAt(index);
            OnValueChanged?.Invoke(Value);
        }

        /// <summary>
        /// Remove all elements from the list.
        /// </summary>
        public void Clear()
        {
            value.Clear();
            OnValueChanged?.Invoke(Value);
        }

        /// <summary>
        /// Remove all elements from the list and add the given element.
        /// </summary>
        public void Overwrite(T element)
        {
            value.Clear();
            value.Add(element);
            OnValueChanged?.Invoke(Value);
        }

        /// <summary>
        /// Remove all elements from the list and add the given list.
        /// </summary>
        public void Overwrite(IEnumerable<T> elements)
        {
            value.Clear();
            value.AddRange(elements);
            OnValueChanged?.Invoke(Value);
        }
        
        /// <summary>
        /// Subscribe to value changes and invoke the given subscriber immediately. Return the unsubscribe action.
        /// </summary>
        public Action SubscribeAndInvoke(Action<IReadOnlyList<T>> subscriber)
        {
            OnValueChanged += subscriber;
            subscriber.Invoke(value);
            
            return () => OnValueChanged -= subscriber;
        }
        
        /// <summary>
        /// Subscribe to value changes and return the unsubscribe action.
        /// </summary>
        public Action Subscribe(Action<IReadOnlyList<T>> subscriber)
        {
            OnValueChanged += subscriber;
            
            return () => OnValueChanged -= subscriber;
        }

        public void Unsubscribe(Action<IReadOnlyList<T>> subscriber)
        {
            OnValueChanged -= subscriber;
        }
    }
}