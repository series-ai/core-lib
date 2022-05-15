using System;

namespace Padoru.Core.DebugConsole
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ConsoleCommandAttribute : Attribute
	{
		public string CommandName;
	}
}
