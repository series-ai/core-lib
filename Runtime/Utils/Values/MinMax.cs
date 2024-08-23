using System;

namespace Padoru.Core.Utils
{
	[Serializable]
	public struct MinMax : IEquatable<MinMax>
	{
		public int Min;
		public int Max;

		public bool Equals(MinMax other)
		{
			return Min == other.Min && Max == other.Max;
		}

		public override bool Equals(object obj)
		{
			if (obj is null)
			{
				return false;
			}

			if (obj.GetType() != GetType())
			{
				return false;
			}
			
			return Equals((MinMax)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Min, Max);
		}

		public override string ToString()
		{
			return $"Min: {Min} | Max {Max}";
		}
	}
}