using Padoru.Core.Utils;
using UnityEngine;

namespace Padoru.Core
{
	public static class MinMaxExtensions
	{
		public static float GetRandomValue(this MinMaxFloat minMax)
		{
			return Random.Range(minMax.Min, minMax.Max);
		}
		
		public static int GetRandomValue(this MinMax minMax)
		{
			return Random.Range(minMax.Min, minMax.Max + 1);
		}
	}
}