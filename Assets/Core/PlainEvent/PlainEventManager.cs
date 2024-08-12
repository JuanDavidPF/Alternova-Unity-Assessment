using System;
using System.Collections.Generic;
using Payosky.Core.Singleton;

namespace Payosky.Core.PlainEvent
{
    public sealed class PlainEventManager : PlainSingleton<PlainEventManager>
    {

        private readonly Dictionary<Type, List<Action<PlainEvent>>> eventListeners = new();


        public PlainEventManager() { }

        public void RegisterListener<T>(Action<T> listener) where T : PlainEvent
        {
            Type eventType = typeof(T);

            if (!eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType] = new List<Action<PlainEvent>>();
            }

            eventListeners[eventType].Add(e => listener((T)e));
        }

        public void UnregisterListener<T>(Action<T> listener) where T : PlainEvent
        {
            Type eventType = typeof(T);

            if (eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType].Remove(e => listener((T)e));
            }
        }

        public void TriggerEvent(PlainEvent plainEvent)
        {
            Type eventType = plainEvent.GetType();

            if (eventListeners.ContainsKey(eventType))
            {
                foreach (var listener in eventListeners[eventType])
                {
                    listener(plainEvent);
                }
            }
        }

        public void ClearAllListeners()
        {
            eventListeners.Clear();
        }

    }//Closes EventManager class
}//Closes Namespace declaration