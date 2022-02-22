using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core
{
    public static class TransformExtentions
    {
        public static void DestroyAllChildren(this Transform transform)
        {
            if (transform == null) return;

            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
