using System;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

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

		public void Deserialize(Type type, ref byte[] bytes, string uri, out object value)
		{
			var tex = new Texture2D(2, 2);
			tex.LoadImage(bytes);
			tex.name = FileUtils.PathFromUri(uri);
			
			if (tex.width % 4 == 0 && tex.height % 4 == 0)
			{
				tex.Compress(true);
			}
			else
			{
				Debug.LogWarning($"Could not compress texture `{texture.name}` because it is not divisible by 4");
			}
            
			var pivot = new Vector2(0.5f, 0.5f);
			var rect = new Rect(0.0f, 0.0f, tex.width, tex.height);
			
			value = Sprite.Create(tex, rect, pivot, 100.0f);
		}
	}
}
