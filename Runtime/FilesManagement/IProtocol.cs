using System.Threading;
using System.Threading.Tasks;

namespace Padoru.Core.Files
{
	public interface IProtocol
	{
		Task<bool> Exists(string uri, CancellationToken token = default);

		Task<object> Read<T>(string uri, string version = null, CancellationToken token = default);

		Task<File<T>> Write<T>(string uri, T value, CancellationToken token = default);

		Task Delete(string uri, CancellationToken token = default);
	}
}
