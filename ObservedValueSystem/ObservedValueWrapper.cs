using System;

namespace Jan.Core
{
    public interface IObservedValueWrapper
    {
        void Listen<T>(Action<T> value);
        void Unlisten<T>(Action<T> value);
    }
}