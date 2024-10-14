using System.Threading;
using System.Threading.Tasks;

namespace Padoru.Core.Files
{
	public interface IProtocol
	{
		Task<bool> Exists(string uri, CancellationToken cancellationToken);

		Task<File<T>> Read<T>(string uri, CancellationToken cancellationToken, string version = null);

		Task<File<T>> Write<T>(string uri, T value, CancellationToken cancellationToken);

		Task Delete(string uri, CancellationToken cancellationToken);
	}
}
