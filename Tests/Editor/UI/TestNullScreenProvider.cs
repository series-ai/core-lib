using System;
using UnityEngine;

namespace Padoru.Core.Tests
{
    public class TestNullScreenProvider : IScreenProvider
    {
        public IScreen GetScreen(Transform parent = null)
        {
            return null;
        }
    }
}