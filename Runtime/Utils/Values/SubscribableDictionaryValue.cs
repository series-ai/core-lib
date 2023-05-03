using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Padoru.Core
{
    public enum DictionaryEvent
    {
        DictionaryChanged = 0,
        KeyAdded = 1,
        KeyRemoved = 2,
        KeyValueChanged = 3,
    }
    
    [Serializable]
    public class SubscribableDictionaryValue<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> innerDictionary = new();

        [field: NonSerialized]
        private event Action<DictionaryEvent, KeyValuePair<TKey, TValue>> OnDictionaryChanged;
        
        public int Count => innerDictionary.Count;

        public bool IsReadOnly => false;

        public ICollection<TKey> Keys => innerDictionary.Keys;

        public ICollection<TValue> Values => innerDictionary.Values;
        
        public TValue this[TKey key]
        {
            get
            {
                return innerDictionary[key];
            }
            set
            {
                innerDictionary[key] = value;
                
                OnDictionaryChanged?.Invoke(DictionaryEvent.KeyValueChanged, 
                    new KeyValuePair<TKey, TValue>(key, value));
            }
        }
        
        /// <summary>
        /// Subscribe to dictionary changes and return the unsubscribe action.
        /// </summary>
        /// <param name="subscriber"></param>
        /// <returns></returns>
        public Action Subscribe(Action<DictionaryEvent, KeyValuePair<TKey, TValue>> subscriber)
        {
            OnDictionaryChanged += subscriber;
            return () => OnDictionaryChanged -= subscriber;
        }
        
        /// <summary>
        /// Unsubscribe from list changes
        /// </summary>
        /// <param name="subscriber"></param>
        public void Unsubscribe(Action<DictionaryEvent, KeyValuePair<TKey, TValue>> subscriber)
        {
            OnDictionaryChanged -= subscriber;
        }
        
        public bool TryAdd(TKey key, TValue value)
        {
            var wasAdded = innerDictionary.TryAdd(key, value);

            if (wasAdded)
            {
                OnDictionaryChanged?.Invoke(DictionaryEvent.KeyAdded, 
                    new KeyValuePair<TKey, TValue>(key, value));
            }

            return wasAdded;
        }
        
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }
        
        public void Add(TKey key, TValue value)
        {
            innerDictionary.Add(key, value);
            
            OnDictionaryChanged?.Invoke(DictionaryEvent.KeyAdded, 
                new KeyValuePair<TKey, TValue>(key, value));
        }

        public void Clear()
        {
            innerDictionary.Clear();
            OnDictionaryChanged?.Invoke(DictionaryEvent.DictionaryChanged, default);
        }
        
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return innerDictionary.Contains(item);
        }
        
        public bool ContainsKey(TKey key)
        {
            return innerDictionary.ContainsKey(key);
        }
        
        public bool ContainsValue(TValue value)
        {
            return innerDictionary.ContainsValue(value);
        }
        
        public bool TryGetValue(TKey key, out TValue value)
        {
            return innerDictionary.TryGetValue(key, out value);
        }
        
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            var index = arrayIndex;
            
            foreach (var keyValuePair in innerDictionary)
            {
                array[index] = new KeyValuePair<TKey, TValue>(keyValuePair.Key, keyValuePair.Value);
                arrayIndex++;
            }
        }

        public void Overwrite(IDictionary<TKey, TValue> elements)
        {
            innerDictionary.Clear();

            foreach (var key in elements.Keys)
            {
                innerDictionary.Add(key, elements[key]);
            }
            
            OnDictionaryChanged?.Invoke(DictionaryEvent.DictionaryChanged, default);
        }
        
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            // TODO: Is this ok?
            return Remove(item.Key);
        }
        
        public bool Remove(TKey key)
        {
            if (innerDictionary.ContainsKey(key))
            {
                var value = innerDictionary[key];
                var wasRemoved = innerDictionary.Remove(key);

                if (wasRemoved)
                {
                    OnDictionaryChanged?.Invoke(DictionaryEvent.KeyRemoved, 
                        new KeyValuePair<TKey, TValue>(key, value));
                }

                return wasRemoved;
            }
            
            return false;
        }
        
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return innerDictionary.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            return new Dictionary<TKey, TValue>(innerDictionary);
        }
    }
}