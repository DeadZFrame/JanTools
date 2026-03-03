using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jan.Events
{
    /// <summary>
    /// The EventManager class provides static methods to manage and dispatch
    /// events throughout an application. It allows for registering,
    /// deregistering, and triggering events in a decoupled manner.
    /// </summary>
    public static class EventManager
    {
        private static readonly Dictionary<string, List<IEventWrapper>> Events = new Dictionary<string, List<IEventWrapper>>();

        /// <summary>
        /// Registers an event listener for a specified event type.
        /// Allows the provided listener object to listen for the specified event and execute the given action when the event is triggered.
        /// </summary>
        /// <param name="listener">The object that listens for the event. This is typically used to associate the event with a specific instance.</param>
        /// <param name="eventType">The type of event to listen for, defined as a string.</param>
        /// <param name="action">The Action delegate that will be invoked when the event is triggered.</param>
        public static void Register(this object listener, string eventType, Action action)
        {
            if (Events.TryGetValue(eventType, out var listeners))
            {
                Event existingEvent = null;
                foreach (var e in listeners.ToArray())
                {
                    if (e.Sender == listener && e is Event evt)
                    {
                        existingEvent = evt;
                        break;
                    }
                }

                if (existingEvent != null)
                {
                    existingEvent.Action += action;
                }
                else
                {
                    var @event = new Event(listener, action);
                    listeners.Add(@event);
                }
            }
            else
            {
                var @event = new Event(listener, action);
                Events.Add(eventType, new List<IEventWrapper> { @event });
            }
        }

        /// <summary>
        /// Stops the specified listener from listening to the given event type with the specified action.
        /// </summary>
        /// <param name="listener">The object that was previously registered as a listener.</param>
        /// <param name="eventType">The event type to stop listening to.</param>
        /// <param name="action">The action that was associated with the listener for the given event type.</param>
        public static void UnRegister(this object listener, string eventType, Action action)
        {
            if (Events.TryGetValue(eventType, out var listeners))
            {
                Event existingEvent = null;
                foreach (var e in listeners.ToArray())
                {
                    if (e.Sender == listener && e is Event evt)
                    {
                        existingEvent = evt;
                        break;
                    }
                }

                if (existingEvent != null)
                {
                    existingEvent.Action -= action;
                    if (existingEvent.IsNull()) listeners.Remove(existingEvent);
                }

                if (listeners.Count == 0) Events.Remove(eventType);
            }
        }

        /// <summary>
        /// Adds a listener for a specific event type with a no-parameter action.
        /// </summary>
        /// <param name="listener">The object that listens to the event.</param>
        /// <param name="eventType">The name of the event to listen for.</param>
        /// <param name="action">The action to be executed when the event is triggered.</param>
        public static void Register<T>(this object listener, string eventType, Action<T> action)
        {
            if (Events.TryGetValue(eventType, out var listeners))
            {
                Event<T> existingEvent = null;
                foreach (var e in listeners.ToArray())
                {
                    if (e.Sender == listener && e is Event<T> evt)
                    {
                        existingEvent = evt;
                        break;
                    }
                }

                if (existingEvent != null)
                {
                    existingEvent.Action += action;
                }
                else
                {
                    var @event = new Event<T>(listener, action);
                    listeners.Add(@event);
                }
            }
            else
            {
                var @event = new Event<T>(listener, action);
                Events.Add(eventType, new List<IEventWrapper> { @event });
            }
        }


        /// <summary>
        /// Stops an object from listening to a specific event of a given type with the specified action.
        /// </summary>
        /// <typeparam name="T">The type of the parameter used in the event.</typeparam>
        /// <param name="listener">The object that has been registered as a listener.</param>
        /// <param name="eventType">The event type to stop listening to.</param>
        /// <param name="action">The specific action to unsubscribe from the event.</param>
        public static void UnRegister<T>(this object listener, string eventType, Action<T> action)
        {
            if (Events.TryGetValue(eventType, out var listeners))
            {
                Event<T> existingEvent = null;
                foreach (var e in listeners.ToArray())
                {
                    if (e.Sender == listener && e is Event<T> evt)
                    {
                        existingEvent = evt;
                        break;
                    }
                }

                if (existingEvent != null)
                {
                    existingEvent.Action -= action;
                    if (existingEvent.IsNull()) listeners.Remove(existingEvent);
                }

                if (listeners.Count == 0) Events.Remove(eventType);
            }
        }

        /// <summary>
        /// Registers a listener to a specific event type and associates an action to be executed when the event is triggered.
        /// </summary>
        /// <param name="eventType">The type of the event to listen for.</param>
        /// <param name="action">The action to execute when the specified event is triggered.</param>
        public static void Register(string eventType, Action action)
        {
            if (Events.TryGetValue(eventType, out var events))
            {
                Event existingEvent = null;
                foreach (var e in events.ToArray())
                {
                    if (e is Event evt)
                    {
                        existingEvent = evt;
                        break;
                    }
                }

                if (existingEvent != null)
                {
                    existingEvent.Action += action;
                }
                else
                {
                    var @event = new Event(action);
                    events.Add(@event);
                }
            }
            else
            {
                var @event = new Event(action);
                Events.Add(eventType, new List<IEventWrapper> { @event });
            }
        }

        /// <summary>
        /// Stops listening to an event of the specified type and unregisters the provided action.
        /// </summary>
        /// <param name="eventType">The type of the event to stop listening to.</param>
        /// <param name="action">The action to unregister from the specified event type.</param>
        public static void UnRegister(string eventType, Action action)
        {
            if (Events.TryGetValue(eventType, out var events))
            {
                Event existingEvent = null;
                foreach (var e in events.ToArray())
                {
                    if (e is Event evt)
                    {
                        existingEvent = evt;
                        break;
                    }
                }

                if (existingEvent != null)
                {
                    existingEvent.Action -= action;
                    if (existingEvent.IsNull()) events.Remove(existingEvent);
                }

                if (events.Count == 0)
                {
                    Events.Remove(eventType);
                }
            }
        }

        /// Adds a listener for the specified event type.
        /// The action is executed when the event is triggered.
        /// <typeparam name="T">The type of the parameter of the event.</typeparam>
        /// <param name="eventType">The name/type of the event to listen for.</param>
        /// <param name="action">The action to execute when the event is triggered.</param>
        public static void Register<T>(string eventType, Action<T> action)
        {
            if (Events.TryGetValue(eventType, out var events))
            {
                Event<T> existingEvent = null;
                foreach (var e in events.ToArray())
                {
                    if (e is Event<T> evt)
                    {
                        existingEvent = evt;
                        break;
                    }
                }

                if (existingEvent != null)
                {
                    existingEvent.Action += action;
                }
                else
                {
                    var @event = new Event<T>(action);
                    events.Add(@event);
                }
            }
            else
            {
                var @event = new Event<T>(action);
                Events.Add(eventType, new List<IEventWrapper> { @event });
            }
        }

        /// <summary>
        /// Stops listening to an event identified by an event type and action.
        /// </summary>
        /// <param name="eventType">The type of the event that the listener wants to stop handling.</param>
        /// <param name="action">The delegate or callback associated with the event type to be removed.</param>
        public static void UnRegister<T>(string eventType, Action<T> action)
        {
            if (Events.TryGetValue(eventType, out var events))
            {
                Event<T> existingEvent = null;
                foreach (var e in events.ToArray())
                {
                    if (e is Event<T> evt)
                    {
                        existingEvent = evt;
                        break;
                    }
                }

                if (existingEvent != null)
                {
                    existingEvent.Action -= action;
                    if (existingEvent.IsNull()) events.Remove(existingEvent);
                }

                if (events.Count == 0)
                {
                    Events.Remove(eventType);
                }
            }
        }

        /// Triggers an event associated with the specified event type.
        /// This method is used to notify all listeners registered to the provided event type.
        /// Any associated raw event or event listeners for the given type will be invoked.
        /// <param name="eventType">
        /// The type of the event to be triggered. This identifies the event for listeners.
        /// </param>
        public static void Trigger(string eventType)
        {
            if (Events.TryGetValue(eventType, out var events))
            {
                foreach (var item in events.ToArray())
                {
                    if(item.Sender != null) continue;
                    if (item is Event existingEvent)
                    {
                        existingEvent.Invoke();
                    }
                }
            }
        }

        /// <summary>
        /// Triggers an event of the specified type with a parameter.
        /// Notifies all listeners subscribed to this event type with the provided parameter.
        /// </summary>
        /// <typeparam name="T">The type of the parameter associated with the event.</typeparam>
        /// <param name="eventType">The type of the event to trigger.</param>
        /// <param name="parameter">The parameter to pass to all listeners of this event type.</param>
        public static void Trigger<T>(string eventType, T parameter)
        {
            if (Events.TryGetValue(eventType, out var events))
            {
                foreach (var item in events.ToArray())
                {
                    if (item.Sender != null) continue;
                    if (item is Event<T> existingEvent)
                    {
                        existingEvent.Invoke(parameter);
                    }
                }
            }
        }

        /// Triggers an event with a parameter for a specific sender, if the sender is registered for the event type.
        /// This method ensures only the listener associated with the given sender is invoked.
        /// <typeparam name="T">The type of the parameter passed to the event.</typeparam>
        /// <param name="sender">The sender object associated with the event listener.</param>
        /// <param name="eventType">The type of the event to trigger.</param>
        /// <param name="parameter">The parameter to pass to the event's listeners.</param>
        public static void Trigger<T>(this object sender, string eventType, T parameter)
        {
            if (Events.TryGetValue(eventType, out var events))
            {
                foreach (var item in events.ToArray())
                {
                    if (item.Sender != null && item is Event<T> existingEvent && existingEvent.Sender.Equals(sender))
                    {
                        existingEvent.Invoke(parameter);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Triggers an event of the specified type but only for listeners associated with a specific sender.
        /// </summary>
        /// <param name="sender">The object that is responsible for triggering the event. Only listeners associated with this sender will be notified.</param>
        /// <param name="eventType">The type of the event to be triggered.</param>
        public static void Trigger(this object sender, string eventType)
        {
            if (Events.TryGetValue(eventType, out var events))
            {
                foreach (var e in events.ToArray())
                {
                    if (e.Sender != null && e.Sender.Equals(sender) && e is Event evt)
                    {
                        evt?.Invoke();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Releases all resources used by the EventManager, clearing all stored event data and resetting internal state.
        /// </summary>
        /// <remarks>
        /// This method clears all registered events, parameterized events, raw events, and parameterized raw events from their respective dictionaries.
        /// It also resets associated debugging tools and tracking flags to their default state.
        /// This method should be called to free resources when the EventManager is no longer needed,
        /// such as when the application or system using it is being shut down or reset.
        /// </remarks>
        public static void Dispose()
        {
            foreach (var value in Events.Values) value.Clear();
            Events.Clear();
        }
    }
}
