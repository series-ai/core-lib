using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;

namespace Padoru.Core.Files
{
	public class PassthroughSerializer : ISerializer
	{
		public Task<byte[]> Serialize(object value)
		{
			var bytes = (byte[])value;
			return Task.FromResult(bytes);
		}

		public Task<object> Deserialize(Type type, byte[] bytes, string uri)
		{
			return Task.FromResult((object)bytes);
		}
	}
}
