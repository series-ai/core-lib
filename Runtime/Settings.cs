using System.Collections.Generic;
using Padoru.Diagnostics;
using UnityEngine;

namespace Padoru.Core
{
	[CreateAssetMenu(menuName = "Padoru/Settings", fileName = "PadoruSettings")]
	public class Settings : ScriptableObject
	{
		public string projectContextPrefabName = "ProjectContext";
		[Tooltip("Only the scenes specified in the list will trigger the initialization of the framework")]
		public List<string> scenes;
		public string initialScene;
		public LogSettings logSettings;
	}
}
