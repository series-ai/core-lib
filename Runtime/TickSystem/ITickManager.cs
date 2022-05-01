namespace Padoru.Core
{
	public interface ITickManager
	{
		void Register(ITickable tickable);

		void Unregister(ITickable tickable);
	}
}
