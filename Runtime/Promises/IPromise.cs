using System;

namespace Padoru.Core
{
	public interface IPromise
	{
		IPromise OnComplete(Action action);
		IPromise OnFail(Action<Exception> action);
		void Complete();
		void Fail(Exception e);
		void Reset();
	}

	public interface IPromise<T>
	{
		IPromise<T> OnComplete(Action<T> action);
		IPromise<T> OnFail(Action<Exception> action);
		void Complete(T result);
		void Fail(Exception e);
		void Reset();
	}
}