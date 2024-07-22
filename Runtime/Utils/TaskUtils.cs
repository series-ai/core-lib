using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Padoru.Core.Utils
{
	public static class TaskUtils
	{
		public static async Task WaitDelay(float delayInSeconds)
		{
			while (delayInSeconds >= 0)
			{
				await Task.Yield();
				
				delayInSeconds -= Time.deltaTime;
			}
		}
		
		// Note that multiple frames might pass between the check and the yield
		public static async Task WaitFrame()
		{
			var current = Time.frameCount;
 
			while (current == Time.frameCount)
			{
				await Task.Yield();
			}
		}

		public static async Task WaitUntil(Func<bool> condition)
		{
			while (!condition())
			{
				await Task.Yield();
			}
		}
		
		// Note that more than the desired amount of frames might pass between the check and the yield
		public static async Task WaitFrames(int framesToWait)
		{
			var target = Time.frameCount + framesToWait;
 
			while (Time.frameCount < target)
			{
				await Task.Yield();
			}
		}
	}
}