using System;
using System.Collections.Generic;
using System.Reflection;
using Event;
using Event.Core;
using Tests;
using UnityEngine;



public class Main : MonoBehaviour
{
    private void Awake()
    {        
        Type eventType = typeof(Event.SampleEvent);
        Debug.Log(eventType.GetType());
        PropertyInfo propertyInfo = eventType.GetProperty("Listeners", BindingFlags.Public | BindingFlags.Static);
        if (propertyInfo != null)
        {
            var listeners = (List<Event.Core.EventListener>)propertyInfo.GetValue(null);
            // Now you can work with the listeners list
            Debug.Log($"Number of listeners: {listeners.Count}");
        }

        EventFactory.Register(new EventSampleStaticTest());
        var evt = new SampleEvent("test", 1);
        EventFactory.Invoke(evt);
        // var lobbyMenu = UIManager.Instanciate<LobbyMenu>();
        // UIManager.LoadWindow(lobbyMenu);
    }
}
