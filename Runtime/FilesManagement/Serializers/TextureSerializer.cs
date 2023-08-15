using System;
using UnityEngine;

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
			texture.Compress(true);
			
			//Debug.LogWarning($"Size {texture.width}, {texture.height}");
			value = texture;
		}
	}
}
