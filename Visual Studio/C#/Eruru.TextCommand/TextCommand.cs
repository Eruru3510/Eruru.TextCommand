using System;
using System.Collections.Generic;
using System.Reflection;

namespace Eruru.TextCommand {

	public class TextCommand<TPermissionLevel> where TPermissionLevel : IComparable {

		public string Id { get; set; }
		public List<string> Names { get; set; } = new List<string> ();
		public object Tag { get; set; }
		public TPermissionLevel PermissionLevel { get; set; }
		public object Instance { get; set; }
		public MethodInfo MethodInfo {

			get => _MethodInfo;

			set {
				_MethodInfo = value;
				ParameterInfo[] parameterInfos = value.GetParameters ();
				int index = TextCommandSystem<object, int>.ParameterOffset;
				Parameters = new TextCommandParameter[parameterInfos.Length - index];
				for (int i = 0; i < Parameters.Length; i++, index++) {
					Parameters[i] = new TextCommandParameter (parameterInfos[index]);
					if (i == Parameters.Length - 1 && parameterInfos[index].ParameterType.IsArray) {
						IsParams = true;
					}
				}
			}

		}
		public TextCommandParameter[] Parameters { get; private set; }
		public bool IsParams { get; private set; }

		MethodInfo _MethodInfo;

		public TextCommand (string id, string name, object tag, TPermissionLevel permissionLevel, MethodInfo methodInfo) {
			if (name is null) {
				throw new ArgumentNullException (nameof (name));
			}
			Id = id ?? throw new ArgumentNullException (nameof (id));
			PermissionLevel = permissionLevel;
			MethodInfo = methodInfo ?? throw new ArgumentNullException (nameof (methodInfo));
			Tag = tag;
			Names.Add (name);
		}
		public TextCommand (string id, string name, object tag, TPermissionLevel permissionLevel, Delegate action) : this (id, name, tag, permissionLevel, action.Method) {
			Instance = action.Target;
		}
		public TextCommand (string id, string[] names, object tag, TPermissionLevel permissionLevel, MethodInfo methodInfo) {
			if (names is null) {
				throw new ArgumentNullException (nameof (names));
			}
			Id = id ?? throw new ArgumentNullException (nameof (id));
			PermissionLevel = permissionLevel;
			MethodInfo = methodInfo ?? throw new ArgumentNullException (nameof (methodInfo));
			Tag = tag;
			Names.AddRange (names);
		}
		public TextCommand (string id, string[] names, object tag, TPermissionLevel permissionLevel, Delegate action) : this (id, names, tag, permissionLevel, action.Method) {
			Instance = action.Target;
		}

	}

}