using System;

namespace Padoru.Core
{
	public class Promise : IPromise
	{
		private bool finished;
		private bool completed;
		private bool failed;
		private Exception exception;
		private Action onCompleteActions;
		private Action<Exception> onFailActions;

		public IPromise OnComplete(Action action)
		{
			if (completed)
			{
				action?.Invoke();
			}
			else if (!finished)
			{
				onCompleteActions += action;
			}

			return this;
		}

		public IPromise OnFail(Action<Exception> action)
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

		public void Complete()
		{
			if (finished)
			{
				throw new Exception("Cannot complete an already finished promise");
			}

			finished = true;
			completed = true;
			onCompleteActions?.Invoke();
			onCompleteActions = null;
		}

		public void Fail(Exception exception)
		{
			if(exception == null)
			{
				throw new Exception("Cannot fail a promise with a null exception");
			}

			if (finished)
			{
				throw new Exception("Cannot fail an already finished promise");
			}

			finished = true;
			failed = true;
			this.exception = exception;
			onFailActions?.Invoke(exception);
			onFailActions = null;
		}

		public void Reset()
		{
			finished = false;
			failed = false;
			completed = false;
			exception = null;
			onCompleteActions = null;
			onCompleteActions = null;
		}
	}
}