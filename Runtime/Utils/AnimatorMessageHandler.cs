using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorMessageHandler : MonoBehaviour
{
    private Dictionary<string, UnityEvent> events = new ();

    public UnityEvent GetEvent(string eventName)
    {
        eventName = eventName.ToLower();

        if (!events.ContainsKey(eventName))
        {
            events.Add(eventName, new UnityEvent());
        }
        
        return events[eventName];
    }

    public void ClearEvents()
    {
        events.Clear();
    }

    public void TriggerEvent(string eventName)
    {
        GetEvent(eventName).Invoke();
    }
}
