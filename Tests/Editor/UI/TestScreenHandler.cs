using UnityEngine;

namespace Padoru.Core.Tests
{
    public class TestScreenHandler : IScreenHandler<TestScreenId>
    {
        public void DisposeScreen(TestScreenId id)
        {
        }

        public IScreen GetScreen(TestScreenId id, Transform parent = null)
        {
            return new TestScreen();
        }
    }
}