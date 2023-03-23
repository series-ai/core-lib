using UnityEngine;

namespace Padoru.Core.Tests
{
    public class TestScreenProvider : IScreenProvider<TestScreenId>
    {
        public IScreen GetScreen(TestScreenId id, Transform parent = null)
        {
            return new TestScreen();
        }
    }
}