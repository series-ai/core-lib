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

		public static void UnregisterService<T>()
		{
			serviceLocator.UnregisterService<T>();
		}

		public static T GetService<T>()
		{
			return serviceLocator.GetService<T>();
		}

		public static void Clear()
		{
			serviceLocator.Clear();
		}
	}
}
