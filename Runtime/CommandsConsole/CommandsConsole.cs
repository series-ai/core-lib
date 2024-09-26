using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.DebugConsole
{
	public class CommandsConsole : MonoBehaviour
	{
		private const string TEXT_FIELD_CONTROL_NAME = "ConsoleTextField";

		private Dictionary<string, InstancedTypeData<ConsoleCommand, ConsoleCommandAttribute>> commands;
		private bool showConsole;
		private string input;

		private void Awake()
		{
			var sb = new StringBuilder();
			sb.Append("Debug Commands Console initialized. Commands:");
			sb.Append(Environment.NewLine);

			var data = AttributeUtils.GetTypesWithAttributeInstanced<ConsoleCommand, ConsoleCommandAttribute>();
			commands = new Dictionary<string, InstancedTypeData<ConsoleCommand, ConsoleCommandAttribute>>();

			foreach (var entry in data)
			{
				var commandName = entry.Attribute.CommandName.ToLower();
				commands.Add(commandName, entry);

				sb.Append($"  - {commandName}");
				sb.Append(Environment.NewLine);
			}

			Debug.Log(sb, Constants.DEBUG_CHANNEL_NAME);
		}

		private void OnGUI()
		{
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.BackQuote)
			{
				ToggleConsole();
			}

			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
			{
				HandleInput();
				input = string.Empty;
				showConsole = false;
			}

			DrawConsole();
			FocusConsole();
		}

		private void DrawConsole()
		{
			if (!showConsole)
			{
				return;
			}

			float y = 0f;

			GUI.Box(new Rect(0, y, Screen.width, 30), "");
			GUI.backgroundColor = new Color(0, 0, 0, 0);

			GUI.SetNextControlName(TEXT_FIELD_CONTROL_NAME);
			input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

			if (input == "`")
			{
				input = string.Empty;
			}
		}

		private void ToggleConsole()
		{
			showConsole = !showConsole;

			if (showConsole)
			{
				input = string.Empty;
			}
		}

		private void FocusConsole()
		{
			if (!showConsole || GUI.GetNameOfFocusedControl() == TEXT_FIELD_CONTROL_NAME)
			{
				return;
			}

			GUI.FocusControl(TEXT_FIELD_CONTROL_NAME);
		}

		private void HandleInput()
		{
			if (!showConsole)
			{
				return;
			}

			var parameters = input.Split(' ');
			if(parameters.Length <= 0)
			{
				return;
			}

			var args = parameters.Skip(1).ToArray();
			var command = parameters[0].ToLower();
			if (commands.ContainsKey(command))
			{
				commands[command].Instance.Execute(args);
			}
		}
	}
}
