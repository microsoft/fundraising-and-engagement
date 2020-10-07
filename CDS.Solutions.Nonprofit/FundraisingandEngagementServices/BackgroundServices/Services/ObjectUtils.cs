using System;
using System.Linq;
using System.Reflection;

namespace FundraisingandEngagement.Utils
{
	[Obsolete]
	public static class ObjectUtils
	{
		// This method will skip CreatedOn and SyncDate
		private static readonly string[] DisallowedProps = new string[2] { "CreatedOn", "SyncDate" };

		public static Tout CopyCommonFieldsTo<Tin, Tout>(this Tin inObj, Tout outObj) where Tout : new()
		{
			if (inObj == null) return outObj;
			outObj = outObj == null ? new Tout() : outObj;

			return inObj.CopyFields(outObj);
		}

		private static Tout CopyFields<Tin, Tout>(this Tin inObj, Tout outObj)
		{
			foreach (var propIn in typeof(Tin).GetProperties().Where(p => p.CanWrite && p.CanRead))
			{
				try
				{
					var propOut = propIn.GetCommonPropInfoMcrm<Tout>();

					if (propOut != null && propOut.CanWrite)
					{
						var value = propIn.GetValue(inObj, null);
						if (value != null && (propIn.PropertyType.Name == propOut.PropertyType.Name) || (propIn.GetGenericPropertyType() == propOut.GetGenericPropertyType()))
						{
							propOut.SetValue(outObj, value);
						}
						else if (propOut.PropertyType.Name == "String")
						{
							propOut.SetValue(outObj, value!.ToString());
						}
					}
				}
				catch
				{
					continue;
				}
			}
			return outObj;
		}

		private static string GetGenericPropertyType(this PropertyInfo prop)
		{
			var type = prop.PropertyType;
			if (type.IsGenericType)
			{
				var itemType = type.GetGenericArguments()[0]; // use this...
				return itemType.Name;
			}
			return type.Name;
		}

		private static PropertyInfo? GetCommonPropInfoMcrm<Tout>(this PropertyInfo propIn)
		{
			if (DisallowedProps.Any(d => d == propIn.Name))
			{
				return null;
			}

			var propOut = typeof(Tout).GetProperty(propIn.Name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (propOut == null)
			{
				propOut = typeof(Tout).GetProperty(propIn.Name.Replace("msnfp_", ""), BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			}
			if (propOut == null)
			{
				propOut = typeof(Tout).GetProperty($"msnfp_{propIn.Name}", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			}
			return propOut;
		}
	}
}