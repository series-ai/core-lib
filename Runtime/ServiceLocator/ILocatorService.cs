using System.Collections.Generic;

namespace Padoru.Core
{
	public interface IServiceLocator
	{
		void Register<T, S>() where S : T, new();

		void Register<T>(T service);

		void Register<T, S>(string tag) where S : T, new();

		void Register<T>(T service, string tag);

		void Unregister<T>();

		void Unregister<T>(string tag);

		T Get<T>();

		T Get<T>(string tag);

		List<T> GetAll<T>();

		void Clear();
	}
}
