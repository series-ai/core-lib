using UnityEngine;

namespace Padoru.Core.Tests
{
    public class TestScreenProvider : IScreenProvider
    {
        public IScreen GetScreen(Transform parent = null)
        {
            return new TestScreen();
        }
    }
}