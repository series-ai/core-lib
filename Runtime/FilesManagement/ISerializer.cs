using System;
using System.Threading;
using System.Threading.Tasks;

namespace Padoru.Core.Files
{
    public interface ISerializer
    {
        Task<byte[]> Serialize(object value, CancellationToken cancellationToken);

        Task<object> Deserialize(Type type, byte[] bytes, string uri, CancellationToken cancellationToken);
    }
}