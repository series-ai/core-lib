using System.Threading;
using System.Threading.Tasks;

namespace Padoru.Core
{
    public interface IScreen
    {
        Task Show(CancellationToken cancellationToken);

        Task Close(CancellationToken cancellationToken);
    }
}