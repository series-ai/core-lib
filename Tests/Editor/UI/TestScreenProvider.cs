using UnityEngine;

namespace Padoru.Core.Tests
{
    public class TestScreenProvider : IScreenProvider
    {
        public IPromise<IScreen> GetScreen(Transform parent = null)
        {
            return PromiseFactory.CreateCompleted<IScreen>(new TestScreen());
        }
    }
}