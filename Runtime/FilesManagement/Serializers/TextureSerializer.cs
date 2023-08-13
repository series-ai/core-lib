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

		public void Deserialize(Type type, ref byte[] bytes, out object value)
		{
			var tex = new Texture2D(2, 2);
			tex.LoadImage(bytes);
            
			value = tex;
		}
	}
}
