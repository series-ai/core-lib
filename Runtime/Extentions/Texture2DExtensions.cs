using UnityEngine;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
	public static class Texture2DExtensions
	{
		public static Sprite ConvertToSprite(this Texture2D texture, bool generateFallbackPhysicsShape = false)
		{
			if (texture == null)
			{
				Debug.LogError("Could not convert null texture into sprite", DebugChannels.TEXTURES);
				return null;
			}
            
			var pivot = new Vector2(0.5f, 0.5f);

			var sprite = Sprite.Create(
				texture, 
				new Rect(0.0f, 0.0f, texture.width, texture.height), 
				pivot, 
				100.0f,
				0U,
				SpriteMeshType.Tight,
				Vector4.zero,
				generateFallbackPhysicsShape);

			sprite.name = texture.name;

			return sprite;
		}
	}
}