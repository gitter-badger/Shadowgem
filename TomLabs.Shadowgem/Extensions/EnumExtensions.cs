﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace TomLabs.Shadowgem.Extensions.Enumeration
{
	/// <summary>
	/// Enum related extension methods
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Returns <see cref="DescriptionAttribute"/> value for enum
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="enumerationValue"></param>
		/// <param name="attributeType"></param>
		/// <returns></returns>
		public static string GetDescription<T, U>(this T enumerationValue, Type attributeType)
			where T : struct
			where U : DescriptionAttribute
		{
			Type type = enumerationValue.GetType();
			if (!type.IsEnum)
			{
				throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
			}

			//Tries to find a DescriptionAttribute for a potential friendly name
			//for the enum
			MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
			if (memberInfo != null && memberInfo.Length > 0)
			{
				object[] attrs = memberInfo[0].GetCustomAttributes(attributeType, false);

				if (attrs != null && attrs.Length > 0)
				{
					//Pull out the description value
					var attribute = attrs[0] as U;
					return attribute.Description;
				}
			}

			return string.Empty;
		}

		/// <summary>
		/// Returns <see cref="DescriptionAttribute"/> value for enum
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerationValue"></param>
		/// <returns></returns>
		public static string GetDescription<T>(this T enumerationValue) where T : struct
		{
			return enumerationValue.GetDescription<T, DescriptionAttribute>(typeof(DescriptionAttribute));
		}

		/// <summary>
		/// https://stackoverflow.com/questions/5542816/printing-flags-enum-as-separate-flags
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static IEnumerable<Enum> GetFlags(this Enum value)
		{
			return GetFlags(value, Enum.GetValues(value.GetType()).Cast<Enum>().ToArray());
		}

		public static IEnumerable<Enum> GetIndividualFlags(this Enum value)
		{
			return GetFlags(value, GetFlagValues(value.GetType()).ToArray());
		}

		private static IEnumerable<Enum> GetFlags(Enum value, Enum[] values)
		{
			ulong bits = Convert.ToUInt64(value);
			List<Enum> results = new List<Enum>();
			for (int i = values.Length - 1; i >= 0; i--)
			{
				ulong mask = Convert.ToUInt64(values[i]);
				if (i == 0 && mask == 0L)
					break;
				if ((bits & mask) == mask)
				{
					results.Add(values[i]);
					bits -= mask;
				}
			}
			if (bits != 0L)
				return Enumerable.Empty<Enum>();
			if (Convert.ToUInt64(value) != 0L)
				return results.Reverse<Enum>();
			if (bits == Convert.ToUInt64(value) && values.Length > 0 && Convert.ToUInt64(values[0]) == 0L)
				return values.Take(1);
			return Enumerable.Empty<Enum>();
		}

		private static IEnumerable<Enum> GetFlagValues(Type enumType)
		{
			ulong flag = 0x1;
			foreach (var value in Enum.GetValues(enumType).Cast<Enum>())
			{
				ulong bits = Convert.ToUInt64(value);
				if (bits == 0L)
					//yield return value;
					continue; // skip the zero value
				while (flag < bits) flag <<= 1;
				if (flag == bits)
					yield return value;
			}
		}
	}
}
