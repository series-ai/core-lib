using System;
using System.Collections.Generic;

namespace Padoru.Core
{
	public class BasicLocator : IServiceLocator
	{
		private Dictionary<Type, object> services = new Dictionary<Type, object>();
		private Dictionary<(string, Type), object> taggedServices = new Dictionary<(string, Type), object>();

		public void Register<T, S>() where S : T, new()
		{
			var service = new S();

			Register<T>(service);
		}

		public void Register<T>(T service)
		{
			if(service == null)
			{
				throw new Exception($"Trying to register a null service");
			}

			var type = typeof(T);
			if (services.ContainsKey(type))
			{
				throw new Exception($"A service of type {type} is already registered");
			}

			services.Add(type, service);
		}

		public void Register<T, S>(string tag) where S : T, new()
		{
			var service = new S();

			Register<T>(service, tag);
		}

		public void Register<T>(T service, string tag)
		{
			if (service == null)
			{
				throw new Exception($"Trying to register a null service");
			}

			var type = typeof(T);
			if (taggedServices.ContainsKey((tag, type)))
			{
				throw new Exception($"A service of type {type} with tag {tag} is already registered");
			}

			taggedServices.Add((tag, type), service);
		}

		public void Unregister<T>()
		{
			var type = typeof(T);
			if (!services.ContainsKey(type))
			{
				throw new Exception($"There is no service of type {type} registered");
			}

			services.Remove(type);
		}

		public void Unregister<T>(string tag)
		{
			var type = typeof(T);
			if (!taggedServices.ContainsKey((tag, type)))
			{
				throw new Exception($"There is no service of type {type} with tag {tag} registered");
			}

			taggedServices.Remove((tag, type));
		}

		public T Get<T>()
		{
			var type = typeof(T);
			if (!services.TryGetValue(type, out var service))
			{
				throw new Exception($"There is no service of type {type} registered");
			}

			return (T)service;
		}

		public T Get<T>(string tag)
		{
			var type = typeof(T);
			if (!taggedServices.TryGetValue((tag, type), out var service))
			{
				throw new Exception($"There is no service of type {type} with tag {tag} registered");
			}

			return (T)service;
		}

		public List<T> GetAll<T>()
		{
			var servicesOfTypeT = new List<T>();

			foreach (var service in services)
			{
				if(typeof(T) == service.Key)
				{
					servicesOfTypeT.Add((T)service.Value);
				}
			}

			foreach (var service in taggedServices)
			{
				if (typeof(T) == service.Key.Item2)
				{
					servicesOfTypeT.Add((T)service.Value);
				}
			}

			return servicesOfTypeT;
		}

		public void Clear()
		{
			services.Clear();
			taggedServices.Clear();
		}
	}
}
