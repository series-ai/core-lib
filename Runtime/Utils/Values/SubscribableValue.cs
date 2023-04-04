using System;

namespace Padoru.Core
{
	[Serializable]
	public class SubscribableValue<T>
	{
		private T value;

		[field: NonSerialized]
		public event Action<T> OnValueChanged;

		public T Value
		{
			get => value;
			set
			{
				if (Equals(this.value, value))
				{
					return;
				}
				
				this.value = value;

				OnValueChanged?.Invoke(this.value);
			}
		}

		public SubscribableValue(T initialValue)
		{
			value = initialValue;
		}
		
		public Action SubscribeAndInvoke(Action<T> subscriber)
		{
			OnValueChanged += subscriber;
			subscriber.Invoke(value);

			return () => OnValueChanged -= subscriber;
		}
		
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
			value = newValue;
			OnValueChanged?.Invoke(newValue);
		}
	}
}
