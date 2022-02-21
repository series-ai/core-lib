using System.Collections.Generic;

namespace Padoru.Core
{
	public interface IServiceLocator
	{
		void RegisterService<T, S>() where S : T, new();

		void RegisterService<T>(T service);

		void RegisterService<T, S>(string tag) where S : T, new();

		void RegisterService<T>(T service, string tag);

		void UnregisterService<T>();

		void UnregisterService<T>(string tag);

		T GetService<T>();

		T GetService<T>(string tag);

		List<T> GetAllServices<T>();

		void Clear();
	}
}
