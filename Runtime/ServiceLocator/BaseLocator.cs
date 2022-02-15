using System;
using System.Collections.Generic;

namespace Padoru.Core
{
	public class BasicLocator : IServiceLocator
	{
		private Dictionary<Type, object> services = new Dictionary<Type, object>();

		public void RegisterService<T, S>() where S : T, new()
		{
			var service = new S();

			RegisterService<T>(service);
		}

		public void RegisterService<T>(T service)
		{
			var type = typeof(T);
			if (services.ContainsKey(type))
			{
				throw new Exception($"A service of type {type} is already registered");
			}

			services.Add(type, service);
		}

		public void UnregisterService<T>()
		{
			var type = typeof(T);
			if (!services.ContainsKey(type))
			{
				throw new Exception($"There is no service of type {type} registered");
			}

			services.Remove(type);
		}

		public T GetService<T>()
		{
			var type = typeof(T);
			if (!services.TryGetValue(type, out var service))
			{
				throw new Exception($"There is no service of type {type} registered");
			}

			return (T)service;
		}

		public void Clear()
		{
			services.Clear();
		}
	}
}
