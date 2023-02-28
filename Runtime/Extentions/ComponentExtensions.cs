using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core
{
    public static class ComponentExtensions
    {
        public static T[] GetComponentsInFirstDepthChildren<T>(this Component component)
        {
            List<T> list = new List<T>();

            for (int i = 0; i < component.transform.childCount; i++)
            {
                var child = component.transform.GetChild(i);
                var type = child.GetComponent<T>();
                if (type != null)
                {
                    list.Add(type);
                }
            }
            
            return list.ToArray();
        }
    }
}
