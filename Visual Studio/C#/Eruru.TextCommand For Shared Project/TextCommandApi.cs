using System;
using System.Collections.Generic;
using System.Reflection;

namespace Eruru.TextCommand {

	static class TextCommandApi {

		public static T GetCustomAttribute<T> (MethodInfo methodInfo) where T : Attribute {
			if (methodInfo is null) {
				throw new ArgumentNullException (nameof (methodInfo));
			}
			object[] attributes = methodInfo.GetCustomAttributes (typeof (T), false);
			return attributes.Length == 0 ? null : (T)attributes[0];
		}

		public static bool Contains (IEnumerable<string> strings, string value, bool ignoreCase = true) {
			if (strings is null) {
				throw new ArgumentNullException (nameof (strings));
			}
			StringComparison stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
			foreach (string current in strings) {
				if (string.Equals (current, value, stringComparison)) {
					return true;
				}
			}
			return false;
		}

		public static TextCommandTokenType GetTokenType (Type type) {
			if (type is null) {
				throw new ArgumentNullException (nameof (type));
			}
			if (type.IsArray) {
				return GetTokenType (type.GetElementType ());
			}
			switch (Type.GetTypeCode (type)) {
				case TypeCode.Byte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				case TypeCode.SByte:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
					return TextCommandTokenType.Integer;
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return TextCommandTokenType.Decimal;
				case TypeCode.Boolean:
					return TextCommandTokenType.Bool;
				case TypeCode.Char:
				case TypeCode.String:
				case TypeCode.DateTime:
					return TextCommandTokenType.String;
				default:
					throw new Exception ($"不支持{type}");
			}
		}

	}

}