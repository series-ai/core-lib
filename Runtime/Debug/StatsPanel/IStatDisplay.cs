using System;

namespace Padoru.Core
{
    public interface IStatDisplay : IDisposable
    {
        string GetStatText();
    }
}