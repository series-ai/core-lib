using System;

namespace Padoru.Core
{
	public class InstancedTypeData<TType, TAttribute> where TType : class where TAttribute : Attribute
	{
		public readonly TType Instance;
		public readonly TAttribute Attribute;

		public InstancedTypeData(TType instance, TAttribute attribute)
		{
			Instance = instance;
			Attribute = attribute;
		}
	}
}
