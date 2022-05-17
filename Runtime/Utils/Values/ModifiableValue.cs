using System.Collections.Generic;

namespace Padoru.Core
{
	public class ModifiableValue<TValue, TCalculator> where TCalculator : ICalculator<TValue>, new()
	{
		private List<ValueModifier<TValue>> modifiers = new List<ValueModifier<TValue>>();
		private TCalculator calculator = new TCalculator();
		private TValue baseValue;

		public TValue Value
		{
			get
			{
				var finalValue = baseValue;
				foreach (var modifier in modifiers)
				{
					finalValue = calculator.Add(finalValue, modifier.Value);
				}

				return finalValue;
			}
		}

		public ModifiableValue(TValue initialValue)
		{
			baseValue = initialValue;
		}

		public void AddModifier(ValueModifier<TValue> modifier)
		{
			if (modifiers.Contains(modifier))
			{
				return;
			}

			modifiers.Add(modifier);
		}

		public void RemoveModifier(ValueModifier<TValue> modifier)
		{
			if (!modifiers.Contains(modifier))
			{
				return;
			}

			modifiers.Remove(modifier);
		}
	}
}
