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

		public static void RegisterService<T, S>() where S : T, new()
		{
			serviceLocator.RegisterService<T, S>();
		}

		public static void RegisterService<T>(T service)
		{
			serviceLocator.RegisterService<T>(service);
		}

		public static void RegisterService<T, S>(string tag) where S : T, new()
		{
			serviceLocator.RegisterService<T, S>(tag);
		}

		public static void RegisterService<T>(T service, string tag)
		{
			serviceLocator.RegisterService<T>(service, tag);
		}

		public static void UnregisterService<T>()
		{
			serviceLocator.UnregisterService<T>();
		}

		public static void UnregisterService<T>(string tag)
		{
			serviceLocator.UnregisterService<T>(tag);
		}

		public static T GetService<T>()
		{
			return serviceLocator.GetService<T>();
		}

		public static T GetService<T>(string tag)
		{
			return serviceLocator.GetService<T>(tag);
		}

		public static List<T> GetAllServices<T>()
		{
			return serviceLocator.GetAllServices<T>();
		}

		public static void Clear()
		{
			serviceLocator.Clear();
		}
	}
}
