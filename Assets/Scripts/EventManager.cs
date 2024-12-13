using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    // Dictionary holding events and their corresponding listeners
    private Dictionary<string, Action<object>> eventDictionary;

    protected override void Awake()
    {
        base.Awake();
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<object>>();
        }
    }

    // Method to start listening to an event
    public static void StartListening(string eventName, Action<object> listener)
    {
        if (Instance.eventDictionary.TryGetValue(eventName, out Action<object> thisEvent))
        {
            thisEvent += listener;
            Instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    // Method to stop listening to an event
    public static void StopListening(string eventName, Action<object> listener)
    {
        if (Instance.eventDictionary.TryGetValue(eventName, out Action<object> thisEvent))
        {
            thisEvent -= listener;
            Instance.eventDictionary[eventName] = thisEvent;
        }
    }

    // Method to trigger an event
    public static void TriggerEvent(string eventName, object eventData = null)
    {
        if (Instance.eventDictionary.TryGetValue(eventName, out Action<object> thisEvent))
        {
            thisEvent.Invoke(eventData);
        }
    }
}
