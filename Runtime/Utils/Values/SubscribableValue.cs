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
			get
			{
				return value;
			}
			set
			{
				this.value = value;

				// The '?' operator doesn't work with the serialization
				if (OnValueChanged != null)
				{
					OnValueChanged.Invoke(this.value);
				}
			}
		}

		public SubscribableValue(T initialValue)
		{
			value = initialValue;
		}
	}
}
