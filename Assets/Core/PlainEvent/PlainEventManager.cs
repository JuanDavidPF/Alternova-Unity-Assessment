using System;
using System.Collections.Generic;
using Payosky.Core.Singleton;

namespace Payosky.Core.PlainEvent
{
    public delegate void EventCallback<T>(T data);
    public sealed class PlainEventManager
    {
        private static Dictionary<Type, List<Delegate>> eventDictionary = new();




        public static void AddEventListener<T>(EventCallback<T> listener)
        {

            List<Delegate> listeners = null;
            if (eventDictionary.TryGetValue(typeof(T), out listeners))
            {
                if (!listeners.Contains(listener))
                {
                    listeners.Add(listener);
                }
            }
            else
            {
                listeners = new List<Delegate>();
                listeners.Add(listener);
                eventDictionary.Add(typeof(T), listeners);
            }
        }

        public static List<Delegate> GetListenersOfEvent<T>()
        {

            List<Delegate> listeners = null;
            if (eventDictionary.TryGetValue(typeof(T), out listeners))
            {
                return listeners;
            }
            else
            {
                return null;
            }
        }

        public static void RemoveEventListener<T>(EventCallback<T> listener)
        {
            List<Delegate> listeners = null;
            if (eventDictionary.TryGetValue(typeof(T), out listeners))
            {
                listeners.Remove(listener);
            }
        }

        public static void RemoveEvent<T>()
        {
            eventDictionary.Remove(typeof(T));
        }

        public static void RemoveAllEvents()
        {
            eventDictionary = new Dictionary<Type, List<Delegate>>();
        }

        public static void TriggerEvent<T>(T data)
        {
            List<Delegate> listeners = null;
            if (eventDictionary.TryGetValue(typeof(T), out listeners))
            {
                foreach (var listener in listeners.ToArray())
                {
                    listener.DynamicInvoke(data);
                }
            }
        }
    }
}