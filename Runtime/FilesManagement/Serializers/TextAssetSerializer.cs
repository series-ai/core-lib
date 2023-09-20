using System;
using System.Text;
using UnityEngine;

namespace Padoru.Core.Files
{
	public class TextAssetSerializer : ISerializer
	{
		public void Serialize(object value, out byte[] bytes)
		{
			var textAsset = (TextAsset)value;
			
			bytes = Encoding.UTF8.GetBytes(textAsset.text);
		}

		public void Deserialize(Type type, ref byte[] bytes, string uri, out object value)
		{
			var text = Encoding.UTF8.GetString(bytes);

			value = new TextAsset(text);
		}
	}
}
