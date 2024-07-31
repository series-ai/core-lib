using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Padoru.Core.Files
{
	public class TextAssetSerializer : ISerializer
	{
		public Task<byte[]> Serialize(object value, CancellationToken cancellationToken)
		{
			var textAsset = (TextAsset)value;
			
			var bytes = Encoding.UTF8.GetBytes(textAsset.text);
			
			return Task.FromResult(bytes);
		}

		public Task<object> Deserialize(Type type, byte[] bytes, string uri, CancellationToken cancellationToken)
		{
			var text = Encoding.UTF8.GetString(bytes);

			object value = new TextAsset(text);
			
			return Task.FromResult(value);
		}
	}
}
