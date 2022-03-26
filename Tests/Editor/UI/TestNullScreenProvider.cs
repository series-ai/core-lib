using System;
using UnityEngine;

namespace Padoru.Core.Tests
{
    public class TestNullScreenProvider : IScreenProvider
    {
        public IPromise<IScreen> GetScreen(Transform parent = null)
        {
            return PromiseFactory.CreateFailed<IScreen>(new Exception());
        }
    }
}