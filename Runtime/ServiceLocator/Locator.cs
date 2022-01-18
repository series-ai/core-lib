using System;
using System.Collections.Generic;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
	public static class Locator
	{
		private static Dictionary<Type, object> services = new Dictionary<Type, object>();

		public static void RegisterService<T, S>() where S : T, new()
		{
			var type = typeof(T);
			if (services.ContainsKey(type))
			{
				throw new Exception($"A service of type {type} is already registered");
			}

			var service = new S();
			services.Add(type, service);
		}

		public static void UnregisterService<T>()
		{
			var type = typeof(T);
			if (!services.ContainsKey(type))
			{
				throw new Exception($"There is no service of type {type} registered");
			}

			services.Remove(type);
		}

		public static T GetService<T>()
		{
			var type = typeof(T);
			if (!services.TryGetValue(type, out var service))
			{
				throw new Exception($"There is no service of type {type} registered");
			}

			return (T)service;
		}

		public static void Clear()
		{
			services.Clear();
		}
	}
}
