using UnityEngine;

namespace Padoru.Core
{
	public static class GameObjectExtensions
	{
		public static void SetLayer(this GameObject gameObject, string layerName, bool includeChildren = false)
		{
			var layer = LayerMask.NameToLayer(layerName);
			gameObject.SetLayer(layer, includeChildren);
		}
		
		public static void SetLayer(this GameObject gameObject, int layer, bool includeChildren = false)
		{
			if (!includeChildren)
			{
				gameObject.layer = layer;
				return;
			}
			
			SetLayerRecursively(gameObject, layer);
		}
		
		private static void SetLayerRecursively(GameObject gameObject, int layer)
		{
			gameObject.layer = layer;
			
			foreach (Transform child in gameObject.transform)
			{
				SetLayerRecursively(child.gameObject, layer);
			}
		}
	}
}
