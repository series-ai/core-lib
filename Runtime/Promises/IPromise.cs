using System;

namespace Padoru.Core
{
	public interface IPromise
	{
		IPromise OnComplete(Action action);
		IPromise OnFail(Action<Exception> action);
	}

	public interface IPromise<T>
	{
		IPromise<T> OnComplete(Action<T> action);
		IPromise<T> OnFail(Action<Exception> action);
	}
}