using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core.DebugConsole
{
    public class CommandsConsoleInitializer : MonoBehaviour, IInitializable, IShutdowneable
    {
        private CommandsConsole commandsConsole;
        private IGUIManager guiManager;
        
        public void Init()
        {
            guiManager = Locator.Get<IGUIManager>();
            
            var data = AttributeUtils.GetTypesWithAttributeInstanced<BaseConsoleCommand, ConsoleCommandAttribute>();
            var commands = new Dictionary<string, BaseConsoleCommand>();

            foreach (var entry in data)
            {
                var commandName = entry.Attribute.CommandName.ToLower();
                commands.Add(commandName, entry.Instance);
            }
            
            var consoleConfig = new CommandsConsoleConfig()
            {
                ToggleConsoleKey = KeyCode.BackQuote,
                HandleInputKey = KeyCode.Return,
                Commands = commands
            };
            
            commandsConsole = new CommandsConsole(consoleConfig);

            guiManager.Register(commandsConsole);
        }

        public void Shutdown()
        {
            guiManager.Unregister(commandsConsole);
        }
    }
}