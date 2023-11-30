using System;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Files
{
	public static class TextureUtils
	{
		public static Texture2D FromBytes(byte[] bytes, string textureName, TextureImportSettings importSettings)
		{
			var texture = new Texture2D(4, 4, importSettings.TextureFormat, importSettings.UseMipMaps);
			texture.LoadImage(bytes);
			texture.name = textureName;
			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = FilterMode.Bilinear;

			// TODO: Stop compressing when loading in the proper format
			if (importSettings.CompressTexture)
			{
				if (texture.width % 4 == 0 && texture.height % 4 == 0)
				{
					texture.Compress(true);
				}
				else
				{
					Debug.LogWarning($"Could not compress texture `{texture.name}` because it is not divisible by 4");
				}
			}

			return texture;
		}
	}
}
