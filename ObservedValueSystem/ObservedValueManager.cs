using System.Collections.Generic;
using UnityEngine;

namespace Jan.Core
{
    public static class ObservedValueManager
    {
        private static readonly Dictionary<string, IObservedValueWrapper> ObservedValues = new Dictionary<string, IObservedValueWrapper>();

        public static void Register(string key, IObservedValueWrapper observedValue)
        {
            ObservedValues[key] = observedValue;

            Debug.Log($"Registered observed value with key: {key}");
        }

        public static IObservedValueWrapper Get(string key)
        {
            ObservedValues.TryGetValue(key, out var observedValue);
            return observedValue;
        }

        public static string[] GetAllObservedNames()
        {
            var keys = new string[ObservedValues.Keys.Count];
            ObservedValues.Keys.CopyTo(keys, 0);
            
            return keys;
        }
    }
}