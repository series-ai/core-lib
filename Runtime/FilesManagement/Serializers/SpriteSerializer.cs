using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Padoru.Core.Files
{
	public class SpriteSerializer : ISerializer
	{
		private readonly IFileNameGenerator fileNameGenerator;
		private readonly TextureImportSettings importSettings;

		public SpriteSerializer(IFileNameGenerator fileNameGenerator, TextureImportSettings importSettings)
		{
			this.fileNameGenerator = fileNameGenerator;
			this.importSettings = importSettings;
		}

		public Task<byte[]> Serialize(object value, CancellationToken cancellationToken)
		{
			var sprite = (Sprite) value;
			var texture = sprite.texture;

			//We need to convert the texture into an uncompressed version in order to encode to PNG
			
			var newTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
			
			newTexture.SetPixels(0,0,texture.width,texture.height,texture.GetPixels());
			
			newTexture.Apply();
			
			var bytes = newTexture.EncodeToPNG();
			
			return Task.FromResult(bytes);
		}

		public Task<object> Deserialize(Type type, byte[] bytes, string uri, CancellationToken cancellationToken)
		{
			var textureName = fileNameGenerator.GetName(uri);

			var texture = TextureUtils.FromBytes(bytes, textureName, importSettings);
            
			var pivot = new Vector2(0.5f, 0.5f);
			var rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
			
			object value = Sprite.Create(texture, rect, pivot, 100.0f);
			
			return Task.FromResult(value);
		}
	}
}
