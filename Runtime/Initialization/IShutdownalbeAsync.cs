using System.Threading;
using System.Threading.Tasks;

namespace Padoru.Core
{
    public interface IShutdownableAsync
    {
        Task Shutdown(CancellationToken token);
    }
}