using System.Threading;
using System.Threading.Tasks;

namespace Padoru.Core.Tests
{
    public class TestScreen : IScreen
    {
        public async Task Close(CancellationToken cancellationToken)
        {
            //await Task.Delay(10, cancellationToken);
        }

        public async Task Show(CancellationToken cancellationToken)
        {
            //await Task.Delay(10, cancellationToken);
        }
    }
}