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
			//-- CS 7.16.2025:
			//-- Testing the effects of more aggressive compression and how it affects quality, GPU/CPU use.
			//-- Specifically interested in large equipments, like face-unisex-unpiece(32MB)
			//-- Specifically interested in large locations, like ExtRoofHotTubSunset(14MB)
			//if (importSettings.CompressTexture)

			//-- Don't waste compute on images that would take less than 1MB of RAM
			if (texture.width <= 256 && texture.height <= 256)
			{
				return texture;
			}
			
			//-- Always try to compress compatible files
			if (texture.width % 4 == 0 && texture.height % 4 == 0)
			{
				texture.Compress(true);
			}
			else
			{
				Debug.LogWarning($"Could not compress texture `{texture.name}` because it is not divisible by 4", DebugChannels.TEXTURES);
			}

			return texture;
		}
	}
}
