using System;

namespace Padoru.Core
{
	public static class PromiseFactory
	{
		public static IPromise CreateCompleted()
		{
			var promise = new Promise();
			promise.Complete();
			return promise;
		}

		public static IPromise<T> CreateCompleted<T>(T result)
		{
			var promise = new Promise<T>();
			promise.Complete(result);
			return promise;
		}

		public static IPromise CreateFailed(Exception exception = null)
		{
			var promise = new Promise();
			promise.Fail(exception);
			return promise;
		}

		public static IPromise<T> CreateFailed<T>(Exception exception = null)
		{
			var promise = new Promise<T>();
			promise.Fail(exception);
			return promise;
		}
	}
}