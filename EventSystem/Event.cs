using System;

namespace Jan.Events
{
    internal interface IEventWrapper
    {
        public object Sender { get; }
    }

    internal class Event : IEventWrapper
    {
        public object Sender { get; private set; }
        public event Action Action;

        public Event(Action action)
        {
            Sender = null;
            Action += action;
        }

        public Event(object listener, Action action)
        {
            Sender = listener;
            Action += action;
        }

        public void Invoke() => Action?.Invoke();
        public bool IsNull() => Action == null;
    }

    internal class Event<T> : IEventWrapper
    {
        public object Sender { get; private set; }
        public event Action<T> Action;

        public Event(Action<T> action)
        {
            Sender = null;
            Action += action;
        }

        public Event(object listener, Action<T> action)
        {
            Sender = listener;
            Action += action;
        }

        public void Invoke(T parameter) => Action?.Invoke(parameter);
        public bool IsNull() => Action == null;
    }
}