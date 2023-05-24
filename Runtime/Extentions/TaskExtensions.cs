using System.Collections;
using System.Threading.Tasks;

namespace Padoru.Core
{
	public static class TaskExtensions
	{
		public static IEnumerator AsCoroutine (this Task task)
		{
			while (!task.IsCompleted)
			{
				yield return null;
			}
			
			// if task is faulted, throws the exception
			task.GetAwaiter().GetResult();
		}
	}
}
