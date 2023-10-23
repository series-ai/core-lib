using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Padoru.Core.Files
{
	public class TextureSerializer : ISerializer
	{
		private readonly IFileNameGenerator fileNameGenerator;
		private readonly TextureImportSettings importSettings;

		public TextureSerializer(IFileNameGenerator fileNameGenerator, TextureImportSettings importSettings)
		{
			this.fileNameGenerator = fileNameGenerator;
			this.importSettings = importSettings;
		}
		
		public Task<byte[]> Serialize(object value)
		{
			var texture = (Texture2D) value;

			//We need to convert the texture into an uncompressed version in order to encode to PNG
			
			var newTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
			
			newTexture.SetPixels(0, 0, texture.width, texture.height, texture.GetPixels());
			
			newTexture.Apply();
			
			var bytes = newTexture.EncodeToPNG();
			
			return Task.FromResult(bytes);
		}

		public Task<object> Deserialize(Type type, byte[] bytes, string uri)
		{
			var textureName = fileNameGenerator.GetName(uri);

			object value = TextureUtils.FromBytes(bytes, textureName, importSettings);
			
			return Task.FromResult(value);
		}
	}
}
