using System;

namespace Padoru.Core.DebugConsole
{
	public abstract class BaseConsoleCommand
	{
		// Declare internal constructor to prevent users to inherit this class from outside the core lib
		internal BaseConsoleCommand() { }
		
		public abstract void Execute(params object[] args);
	
		protected T ParseArgument<T>(object arg)
		{
			return (T)arg;
		}
	}
	
	public abstract class ConsoleCommand<T1, T2, T3, T4> : BaseConsoleCommand
	{
		protected abstract void Execute(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

		public override void Execute(params object[] args)
		{
			if (args.Length < 4)
			{
				throw new ArgumentException("One or more arguments are missing.");
			}

			var arg1 = ParseArgument<T1>(args[0]);
			var arg2 = ParseArgument<T2>(args[1]);
			var arg3 = ParseArgument<T3>(args[2]);
			var arg4 = ParseArgument<T4>(args[3]);

			Execute(arg1, arg2, arg3, arg4);
		}
	}
	
	public abstract class ConsoleCommand<T1, T2, T3> : BaseConsoleCommand
	{
		protected abstract void Execute(T1 arg1, T2 arg2, T3 arg3);

		public override void Execute(params object[] args)
		{
			if (args.Length < 3)
			{
				throw new ArgumentException("One or more arguments are missing.");
			}

			var arg1 = ParseArgument<T1>(args[0]);
			var arg2 = ParseArgument<T2>(args[1]);
			var arg3 = ParseArgument<T3>(args[2]);

			Execute(arg1, arg2, arg3);
		}
	}
	
	public abstract class ConsoleCommand<T1, T2> : BaseConsoleCommand
	{
		protected abstract void Execute(T1 arg1, T2 arg2);

		public override void Execute(params object[] args)
		{
			if (args.Length < 2)
			{
				throw new ArgumentException("One or more arguments are missing.");
			}

			var arg1 = ParseArgument<T1>(args[0]);
			var arg2 = ParseArgument<T2>(args[1]);

			Execute(arg1, arg2);
		}
	}
	
	public abstract class ConsoleCommand<T1> : BaseConsoleCommand
	{
		protected abstract void Execute(T1 arg1);

		public override void Execute(params object[] args)
		{
			if (args.Length < 1)
			{
				throw new ArgumentException("One or more arguments are missing.");
			}

			var arg1 = ParseArgument<T1>(args[0]);

			Execute(arg1);
		}
	}
	
	
	public abstract class ConsoleCommand : BaseConsoleCommand
	{
		protected abstract void Execute();

		public override void Execute(params object[] args)
		{
			Execute();
		}
	}
}
