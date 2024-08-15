using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.DebugConsole
{
	[ConsoleCommand(CommandName = "log")]
	public class LogCommand : ConsoleCommand<string>
	{
		protected override void Execute(string text)
		{
			Debug.Log(text, DebugChannels.COMMANDS_CONSOLE);
		}
	}
}
