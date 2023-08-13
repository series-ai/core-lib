using System;
using UnityEngine;

namespace Padoru.Core.Files
{
	public class SpriteSerializer : ISerializer
	{
		public void Serialize(object value, out byte[] bytes)
		{
			var sprite = (Sprite) value;
			var texture = sprite.texture;

			bytes = texture.EncodeToPNG();
		}

		public void Deserialize(Type type, ref byte[] bytes, out object value)
		{
			var tex = new Texture2D(2, 2);
			tex.LoadImage(bytes);
            
			var pivot = new Vector2(0.5f, 0.5f);
			var rect = new Rect(0.0f, 0.0f, tex.width, tex.height);
			
			value = Sprite.Create(tex, rect, pivot, 100.0f);
		}
	}
}
