namespace Padoru.Core
{
	public struct IntCalculator : ICalculator<int>
	{
		public int Add(int a, int b)
		{
			return a + b;
		}
	}
}
