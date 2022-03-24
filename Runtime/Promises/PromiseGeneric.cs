using System;

namespace Padoru.Core
{
	public class Promise<T> : IPromise<T>
	{
		private bool finished;
		private bool completed;
		private bool failed;
		private T result;
		private Exception exception;
		private Action<T> onCompleteActions;
		private Action<Exception> onFailActions;

		public IPromise<T> OnComplete(Action<T> action)
		{
			if (completed)
			{
				action?.Invoke(result);
			}
			else if (!finished)
			{
				onCompleteActions += action;
			}

			return this;
		}

		public IPromise<T> OnFail(Action<Exception> action)
		{
			if (failed)
			{
				action?.Invoke(exception);
			}
			else if (!finished)
			{
				onFailActions += action;
			}

			return this;
		}

		public void Complete(T result)
		{
			if (finished)
			{
				throw new Exception("Cannot complete an already finished promise");
			}

			finished = true;
			completed = true;
			this.result = result;
			onCompleteActions?.Invoke(result);
			onCompleteActions = null;
		}

		public void Fail(Exception exception)
		{
			if (finished)
			{
				throw new Exception("Cannot fail an already finished promise");
			}

			finished = true;
			failed = true;
			onFailActions?.Invoke(exception);
			onFailActions = null;
		}

		public void Reset()
		{
			finished = false;
			failed = false;
			completed  = false;
			result = default;
			exception = null;
			onCompleteActions = null;
			onCompleteActions = null;
		}
	}
}