namespace Padoru.Core
{
	public struct FloatCalculator : ICalculator<float>
	{
		public float Add(float a, float b)
		{
			return a + b;
		}
	}
}
