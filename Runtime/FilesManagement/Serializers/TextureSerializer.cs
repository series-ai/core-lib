using System;
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
		
		public void Serialize(object value, out byte[] bytes)
		{
			var texture = (Texture2D) value;

			bytes = texture.EncodeToPNG();
		}

		public void Deserialize(Type type, ref byte[] bytes, string uri, out object value)
		{
			var textureName = fileNameGenerator.GetName(uri);

			value = TextureUtils.FromBytes(bytes, textureName, importSettings);
		}
	}
}
