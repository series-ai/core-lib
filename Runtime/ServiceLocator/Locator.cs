using System.Collections.Generic;

namespace Padoru.Core
{
	public static class Locator
	{
		private static IServiceLocator serviceLocator = new BasicLocator();

		public static void SetLocatorService(IServiceLocator serviceLocator)
		{
			Locator.serviceLocator = serviceLocator;
		}

		public static void Register<T, S>() where S : T, new()
		{
			serviceLocator.Register<T, S>();
		}

		public static void Register<T>(T service)
		{
			serviceLocator.Register<T>(service);
		}

		public static void Register<T, S>(string tag) where S : T, new()
		{
			serviceLocator.Register<T, S>(tag);
		}

		public static void Register<T>(T service, string tag)
		{
			serviceLocator.Register<T>(service, tag);
		}

		public static void Unregister<T>()
		{
			serviceLocator.Unregister<T>();
		}

		public static void Unregister<T>(string tag)
		{
			serviceLocator.Unregister<T>(tag);
		}

		public static T Get<T>()
		{
			return serviceLocator.Get<T>();
		}

		public static T Get<T>(string tag)
		{
			return serviceLocator.Get<T>(tag);
		}

		public static List<T> GetAll<T>()
		{
			return serviceLocator.GetAll<T>();
		}

		public static bool Has<T>()
		{
			return serviceLocator.Has<T>();
		}

		public static bool Has<T>(string tag)
		{
			return serviceLocator.Has<T>(tag);
		}

		public static void Clear()
		{
			serviceLocator.Clear();
		}
	}
}
