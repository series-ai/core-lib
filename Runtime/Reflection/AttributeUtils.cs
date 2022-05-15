using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Padoru.Core
{
	public static class AttributeUtils
	{
		public static List<MemberInfo> GetMembersWithAttribute<T>(BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) where T : Attribute
		{
			var membersWithAttribute = new List<MemberInfo>();
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (var assembly in assemblies)
			{
				var types = assembly.GetTypes();

				foreach (var type in types)
				{
					var members = type.GetMembers(flags);

					foreach (var member in members)
					{
						if (member.CustomAttributes.ToArray().Length > 0)
						{
							var attribute = member.GetCustomAttribute<T>();

							if (attribute != null)
							{
								membersWithAttribute.Add(member);
							}
						}
					}
				}
			}

			return membersWithAttribute;
		}

		public static List<Type> GetTypesWithAttribute<T>() where T : Attribute
		{
			var typersWithAttribute = new List<Type>();
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (var assembly in assemblies)
			{
				var types = assembly.GetTypes();

				foreach (var type in types)
				{
					if (type.CustomAttributes.ToArray().Length > 0)
					{
						var attribute = type.GetCustomAttribute<T>();

						if (attribute != null)
						{
							typersWithAttribute.Add(type);
						}
					}
				}
			}

			return typersWithAttribute;
		}

		public static List<InstancedTypeData<TType, TAttribute>> GetTypesWithAttributeInstanced<TType, TAttribute>() where TAttribute : Attribute where TType : class
		{
			var instances = new List<InstancedTypeData<TType, TAttribute>>();
			var types = GetTypesWithAttribute<TAttribute>();

			foreach (var type in types)
			{
				if (type.Equals(typeof(TType)) || type.IsSubclassOf(typeof(TType)))
				{
					var attribute = type.GetCustomAttribute<TAttribute>();
					var instance = Activator.CreateInstance(type) as TType;
					var data = new InstancedTypeData<TType, TAttribute>(instance, attribute);
					instances.Add(data);
				}
			}

			return instances;
		}
	}
}
