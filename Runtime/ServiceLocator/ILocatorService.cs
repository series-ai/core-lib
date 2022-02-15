namespace Padoru.Core
{
	public interface IServiceLocator
	{
		void RegisterService<T, S>() where S : T, new();

		void RegisterService<T>(T service);

		void UnregisterService<T>();

		T GetService<T>();

		void Clear();
	}
}
