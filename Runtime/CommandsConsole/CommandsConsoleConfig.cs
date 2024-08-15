using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core.DebugConsole
{
    public class CommandsConsoleConfig
    {
        public KeyCode ToggleConsoleKey = KeyCode.BackQuote;
        public KeyCode HandleInputKey = KeyCode.Return;
        public Dictionary<string, BaseConsoleCommand> Commands = new();
    }
}