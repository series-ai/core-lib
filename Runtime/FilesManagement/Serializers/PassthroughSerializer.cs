using System;
using System.Threading;
using System.Threading.Tasks;

namespace Padoru.Core.Files
{
	public class PassthroughSerializer : ISerializer
	{
		public Task<byte[]> Serialize(object value, CancellationToken cancellationToken)
		{
			return Task.FromResult((byte[])value);
		}

		public Task<object> Deserialize(Type type, byte[] bytes, string uri, CancellationToken cancellationToken)
		{
			return Task.FromResult((object)bytes);
		}
	}
}
