namespace Padoru.Core
{
	public interface ICalculator<T>
	{
		T Add(T a, T b);
	}
}