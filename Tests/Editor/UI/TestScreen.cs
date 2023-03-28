using System.Threading.Tasks;

namespace Padoru.Core.Tests
{
    public class TestScreen : IScreen
    {
        public async Task Close()
        {
            await Task.Delay(10);
        }

        public async Task Show()
        {
            await Task.Delay(10);
        }
    }
}