using System;
using System.Reflection;

namespace Eruru.TextCommand {

	static class ExtensionMethods {

		public static T GetAttribute<T> (this MemberInfo memberInfo) where T : Attribute {
			if (memberInfo is null) {
				throw new ArgumentNullException (nameof (memberInfo));
			}
			object[] attributes = memberInfo.GetCustomAttributes (typeof (T), true);
			return attributes.Length == 0 ? null : (T)attributes[0];
		}

		public static int ToInt (this Enum @enum) {
			if (@enum is null) {
				throw new ArgumentNullException (nameof (@enum));
			}
			return @enum.GetHashCode ();
		}

	}

}