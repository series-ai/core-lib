using System;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Files
{
	public class TextureSerializer : ISerializer
	{
		public void Serialize(object value, out byte[] bytes)
		{
			var texture = (Texture2D) value;

			bytes = texture.EncodeToPNG();
		}

		public void Deserialize(Type type, ref byte[] bytes, string uri, out object value)
		{
			var texture = new Texture2D(2, 2);
			texture.LoadImage(bytes);
			texture.name = FileUtils.PathFromUri(uri);
			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = FilterMode.Bilinear;
			
			if (texture.width % 4 == 0 && texture.height % 4 == 0)
			{
				texture.Compress(true);
			}
			else
			{
				Debug.LogWarning($"Could not compress texture `{texture.name}` because it is not divisible by 4");
			}
			
			value = texture;
		}
	}
}
