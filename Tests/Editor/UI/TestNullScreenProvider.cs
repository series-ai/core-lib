using System;
using UnityEngine;

namespace Padoru.Core.Tests
{
    public class TestNullScreenProvider : IScreenProvider<TestScreenId>
    {
        public IScreen GetScreen(TestScreenId id, Transform parent = null)
        {
            return null;
        }
    }
}