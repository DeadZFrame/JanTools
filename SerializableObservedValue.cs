using System;
using Jan.Events;
using UnityEngine;

namespace Jan.Core
{
    [Serializable]
    public struct SerializableObservedValue<T> where T : struct
    {
        [SerializeField] private ObservedValue<T> observedValue;

        public SerializableObservedValue(T initialValue, object sender)
        {
            observedValue = new ObservedValue<T>(initialValue);
            EventManager.Trigger(sender, EventNames.OnValueObserved, initialValue);
        }

        // Property to access ObservedValue<T>
        public readonly ObservedValue<T> Value => observedValue;

        public void SetValue(T value, object sender)
        {
            if (observedValue.Value.Equals(value))
            {
                return;
            }

            var newValue = observedValue;
            newValue.Set(value, sender);
            observedValue = newValue;
        }

        public readonly T GetValue()
        {
            return Value.Value;
        }
    }
}
