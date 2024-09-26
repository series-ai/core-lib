using System;
using System.Collections.Generic;
using Padoru.Diagnostics;

namespace Padoru.Core.ActionRouter
{
    public class ActionRouterGroup
    {
        public string ActionId { get; }

        private readonly List<Action<object>> subscribers = new();
        
        public ActionRouterGroup(string actionId)
        {
            ActionId = actionId;
        }

        public void Invoke(object actionObject)
        {
            for (var index = subscribers.Count - 1; index >= 0; index--)
            {
                var subscriber = subscribers[index];
                subscriber.Invoke(actionObject);
            }
        }

        public void AddSubscriber(Action<object> subscriber)
        {
            if (subscribers.Contains(subscriber))
            {
                Debug.LogWarning($"Unable to subscribe to the action '{ActionId}' because is already subscribed", Constants.DEBUG_CHANNEL_NAME);
                return;
            }
            
            subscribers.Add(subscriber);
        }

        public void RemoveSubscriber(Action<object> subscriber)
        {
            if (!subscribers.Remove(subscriber))
            {
                Debug.LogWarning($"Unable to remove subscriber from event '{ActionId}' because it is not subscribed", Constants.DEBUG_CHANNEL_NAME);
            }
        }
    }
}