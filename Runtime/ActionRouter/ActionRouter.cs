using System;
using System.Collections.Generic;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.ActionRouter
{
    public class ActionRouter : IActionRouter
    {
        private readonly List<ActionRouterGroup> actionRouterGroups = new();
        
        public void Subscribe(string actionId, Action<object> subscriber)
        {
            var actionRouterGroup = GetActionRouterGroup(actionId);

            if (actionRouterGroup == null)
            {
                actionRouterGroup = new ActionRouterGroup(actionId);
                actionRouterGroups.Add(actionRouterGroup);
            }

            actionRouterGroup.AddSubscriber(subscriber);
        }

        public void Unsubscribe(string actionId, Action<object> subscriber)
        {
            var actionGroup = GetActionRouterGroup(actionId);

            if (actionGroup == null)
            {
                Debug.LogWarning($"Unable to remove the subscriber because the action '{actionId}' is not registered", DebugChannels.ACTION_ROUTER);
                return;
            }
            
            actionGroup.RemoveSubscriber(subscriber);
        }

        public void Invoke(string actionId, object actionObject)
        {
            var actionGroup = GetActionRouterGroup(actionId);

            if (actionGroup == null)
            {
                return;
            }
            
            actionGroup.Invoke(actionObject);
        }

        public bool IsActionSubscribed(string actionId)
        {
            var actionGroup = GetActionRouterGroup(actionId);
			
			return actionGroup != null;
        }

        private ActionRouterGroup GetActionRouterGroup(string actionId)
        {
            foreach (var actionRouterGroup in actionRouterGroups)
            {
                if (actionRouterGroup.ActionId == actionId)
                {
                    return actionRouterGroup;
                }
            }
            
            return null;
        }
    }
}