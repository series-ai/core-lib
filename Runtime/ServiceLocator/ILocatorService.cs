namespace Padoru.Core
{
	public interface IServiceLocator
	{
		void RegisterService<T, S>() where S : T, new();

		void UnregisterService<T>();

		T GetService<T>();

		void Clear();
	}
}
