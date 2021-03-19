using System;
using System.Collections.Generic;
using System.Reflection;

namespace Eruru.TextCommand {

	public class TextCommand<TPermissionLevel> where TPermissionLevel : Enum {

		public List<string> Names { get; } = new List<string> ();
		public object Tag { get; set; }
		public TPermissionLevel PermissionLevel { get; }

		internal object Instance { get; }
		internal MethodInfo MethodInfo { get; }
		internal TextCommandParameter[] Parameters { get; }
		internal bool IsParams { get; }

		internal TextCommand (IEnumerable<string> names, object tag, TPermissionLevel permissionLevel, object instance, MethodInfo methodInfo, TextCommandParameter[] parameters, bool isParams) {
			if (names is null) {
				throw new ArgumentNullException (nameof (names));
			}
			if (methodInfo is null) {
				throw new ArgumentNullException (nameof (methodInfo));
			}
			Names.AddRange (names);
			Tag = tag;
			PermissionLevel = permissionLevel;
			Instance = instance;
			MethodInfo = methodInfo;
			Parameters = parameters ?? throw new ArgumentNullException (nameof (parameters));
			IsParams = isParams;
		}

	}

}