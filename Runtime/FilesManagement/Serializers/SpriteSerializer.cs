using System;
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

		public void Serialize(object value, out byte[] bytes)
		{
			var sprite = (Sprite) value;
			var texture = sprite.texture;

			bytes = texture.EncodeToPNG();
		}

		public void Deserialize(Type type, ref byte[] bytes, string uri, out object value)
		{
			var textureName = fileNameGenerator.GetName(uri);

			var texture = TextureUtils.FromBytes(bytes, textureName, importSettings);
            
			var pivot = new Vector2(0.5f, 0.5f);
			var rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
			
			value = Sprite.Create(texture, rect, pivot, 100.0f);
		}
	}
}
