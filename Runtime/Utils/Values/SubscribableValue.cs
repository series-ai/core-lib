using System;

namespace Padoru.Core
{
	public class SubscribableValue<T>
	{
		private T value;

		public event Action<T> OnValueChanged;

		public T Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
				OnValueChanged?.Invoke(this.value);
			}
		}

		public SubscribableValue(T initialValue)
		{
			value = initialValue;
		}
	}
}
