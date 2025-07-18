using System;
using System.Threading;
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
		
		public Task<byte[]> Serialize(object value, CancellationToken cancellationToken)
		{
			var texture = (Texture2D) value;

			//We need to convert the texture into an uncompressed version in order to encode to PNG
			
			var newTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
			
			try
			{
				newTexture.SetPixels(0, 0, texture.width, texture.height, texture.GetPixels());
				newTexture.Apply();
        
				var bytes = newTexture.EncodeToPNG();
				return Task.FromResult(bytes);
			}
			finally
			{
				//We need to destroy this temp object since unity doesn't automatically collect it

				UnityEngine.Object.DestroyImmediate(newTexture);
			}
		}

		public Task<object> Deserialize(Type type, byte[] bytes, string uri, CancellationToken cancellationToken)
		{
			var textureName = fileNameGenerator.GetName(uri);

			object value = TextureUtils.FromBytes(bytes, textureName, importSettings);
			
			return Task.FromResult(value);
		}
	}
}
