using System;
using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core
{
    [Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();
        [SerializeField, HideInInspector] private bool addedItemThroughEditor;

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();

            if (keys.Count != values.Count)
            {
                throw new Exception($"There are {keys.Count} keys and {values.Count} values after deserialization. Make sure that both key and value types are serializable.");
            }

            if (addedItemThroughEditor)
            {
                addedItemThroughEditor = false;

                keys[keys.Count - 1] = GetDefaultKey();
            }

            for (int i = 0; i < keys.Count; i++)
            {
                Add(keys[i], values[i]);
            }
        }

        private TKey GetDefaultKey()
        {
            var type = typeof(TKey);
            if (type.IsValueType)
            {
                var instance = Activator.CreateInstance(type);
                return (TKey)instance;
            }
            // String is a special case because it is not a value type, but it's default is null instead of empty
            else if(type.Equals(typeof(string)))
            {
                return (TKey)Convert.ChangeType(string.Empty, typeof(TKey));
            }

            return default;
        }
    }
}