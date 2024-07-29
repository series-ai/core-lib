using System.Threading;
using System.Threading.Tasks;

namespace Padoru.Core
{
    public interface IInitializableAsync
    {
        Task Init(CancellationToken token);
    }
}