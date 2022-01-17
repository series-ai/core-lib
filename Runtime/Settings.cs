using UnityEngine;

using LogType = Padoru.Diagnostics.LogType;

namespace Padoru.Core
{
	[CreateAssetMenu(menuName = "Padoru/Settings", fileName = "PadoruSettings")]
	public class Settings : ScriptableObject
	{
		[Header("Project initialization")]
		public string ProjectContextPrefabName = "ProjectContext";

		[Header("Logging")]
		public LogType LogType = LogType.Info;
		public LogType StacktraceLogType = LogType.Info;
	}
}
