using System;

namespace Padoru.Core
{
	[Serializable]
	public class SubscribableValue<T>
	{
		private T value;

		[field: NonSerialized]
		private event Action<T> OnValueChanged;

		/// <summary>
		/// Value property that invoke the OnValueChanged event when the value is set.
		/// If you set the same value the OnValueChanged event is not invoked.
		/// </summary>
		public T Value
		{
			get
			{
				return value;
			}
			set
			{
				if (Equals(this.value, value))
				{
					return;
				}

				SetValueAndInvoke(value);
			}
		}

		public SubscribableValue(T initialValue)
		{
			value = initialValue;
		}
		
		/// <summary>
		/// Subscribe to value changes and invoke the given subscriber immediately. Return the unsubscribe action.
		/// </summary>
		public Action SubscribeAndInvoke(Action<T> subscriber)
		{
			OnValueChanged += subscriber;
			subscriber.Invoke(value);

			return () => OnValueChanged -= subscriber;
		}
		
		/// <summary>
		/// Subscribe to value changes and return the unsubscribe action.
		/// </summary>
		/// <param name="subscriber"></param>
		/// <returns></returns>
		public Action Subscribe(Action<T> subscriber)
		{
			OnValueChanged += subscriber;
            
			return () => OnValueChanged -= subscriber;
		}
		
		/// <summary>
		/// Set value and invoke OnValueChanged without checking for changes.
		/// </summary>
		public void ForceValueChange(T newValue)
		{
			SetValueAndInvoke(newValue);
		}

		private void SetValueAndInvoke(T newValue)
		{
			value = newValue;
			OnValueChanged?.Invoke(value);
		}
	}
}
