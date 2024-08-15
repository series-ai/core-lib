using System;

namespace Padoru.Core
{
    public interface IInfoDisplay : IDisposable
    {
        string GetInfoText();
    }
}