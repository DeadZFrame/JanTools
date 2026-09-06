using System;
using Jan.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jan.Core
{
    /// <summary>
    /// Wraps a variable in a class that triggers an
    /// event if the value changes. This is useful when
    /// values can be meaningfully compared using Equals,
    /// and when the variable changes infrequently in
    /// comparison to the number of times it is updated.
    
    [Serializable]
    public class ObservedValue<T> : IObservedValueWrapper where T : struct
    {
        [SerializeField] private string observedValueName;
        [SerializeField] private T currentValue;
        public T lastValue { get; private set; }

        public ObservedValue(T initialValue)
        {
            currentValue = initialValue;
            lastValue = currentValue;

            //ObservedValueManager.Register(observedValueName, this);
        }

        public T Value => currentValue;

        public void Set(T value)
        {
            //if (!currentValue.Equals(value))
            {
                lastValue = currentValue;
                currentValue = value;

                this.Trigger(EventNames.OnValueObserved, value);
                this.Trigger(EventNames.OnValueObserved);
            }
        }

        /// <summary>
        /// Sets the value without notification.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetSilently(T value)
        {
            currentValue = value;
        }

        public void Listen<T>(Action<T> callback)
        {
            this.Register<T>(EventNames.OnValueObserved, callback);
        }

        public void Unlisten<T>(Action<T> callback)
        {
            this.UnRegister<T>(EventNames.OnValueObserved, callback);
        }

        // [Button]
        // private void Register()
        // {
        //     ObservedValueManager.Register(observedValueName, this);
        // }
    }
}