using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Eruru.ReaderWriterLockHelper;
using Eruru.TextTokenizer;

namespace Eruru.TextCommand {

	public class TextCommandSystem<Sender, PermissionLevel> where PermissionLevel : Enum {

		public bool IgnoreCase { get; set; } = true;
		public event Action<Sender> PermissionError;

		readonly ReaderWriterLockHelper<Dictionary<string, TextCommand<PermissionLevel>>> ReaderWriterLockHelper =
			new ReaderWriterLockHelper<Dictionary<string, TextCommand<PermissionLevel>>> (new Dictionary<string, TextCommand<PermissionLevel>> ());

		const int Offset = 1;

		public TextCommand<PermissionLevel> Add (string id, string[] names, PermissionLevel permissionLevel, object instance, string methodName, params Type[] types) {
			if (id is null) {
				throw new ArgumentNullException (nameof (id));
			}
			if (names is null) {
				throw new ArgumentNullException (nameof (names));
			}
			if (instance is null) {
				throw new ArgumentNullException (nameof (instance));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			if (types is null) {
				throw new ArgumentNullException (nameof (types));
			}
			return Add (id, names, null, permissionLevel, instance.GetType (), instance, methodName, types);
		}
		public TextCommand<PermissionLevel> Add (string id, string[] names, object tag, PermissionLevel permissionLevel, object instance, string methodName, params Type[] types) {
			if (id is null) {
				throw new ArgumentNullException (nameof (id));
			}
			if (names is null) {
				throw new ArgumentNullException (nameof (names));
			}
			if (instance is null) {
				throw new ArgumentNullException (nameof (instance));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			if (types is null) {
				throw new ArgumentNullException (nameof (types));
			}
			return Add (id, names, tag, permissionLevel, instance.GetType (), instance, methodName, types);
		}
		public TextCommand<PermissionLevel> Add<T> (string id, string[] names, PermissionLevel permissionLevel, string methodName, params Type[] types) {
			if (names is null) {
				throw new ArgumentNullException (nameof (names));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			if (types is null) {
				throw new ArgumentNullException (nameof (types));
			}
			return Add (id, names, null, permissionLevel, typeof (T), null, methodName, types);
		}
		public TextCommand<PermissionLevel> Add<T> (string id, string[] names, object tag, PermissionLevel permissionLevel, string methodName, params Type[] types) {
			if (names is null) {
				throw new ArgumentNullException (nameof (names));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			if (types is null) {
				throw new ArgumentNullException (nameof (types));
			}
			return Add (id, names, tag, permissionLevel, typeof (T), null, methodName, types);
		}
		public TextCommand<PermissionLevel> Add (string id, string name, PermissionLevel permissionLevel, object instance, string methodName, params Type[] types) {
			if (name is null) {
				throw new ArgumentNullException (nameof (name));
			}
			if (instance is null) {
				throw new ArgumentNullException (nameof (instance));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			if (types is null) {
				throw new ArgumentNullException (nameof (types));
			}
			return Add (id, new string[] { name }, null, permissionLevel, instance.GetType (), instance, methodName, types);
		}
		public TextCommand<PermissionLevel> Add (string id, string name, object tag, PermissionLevel permissionLevel, object instance, string methodName, params Type[] types) {
			if (name is null) {
				throw new ArgumentNullException (nameof (name));
			}
			if (instance is null) {
				throw new ArgumentNullException (nameof (instance));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			if (types is null) {
				throw new ArgumentNullException (nameof (types));
			}
			return Add (id, new string[] { name }, tag, permissionLevel, instance.GetType (), instance, methodName, types);
		}
		public TextCommand<PermissionLevel> Add<T> (string id, string name, PermissionLevel permissionLevel, string methodName, params Type[] types) {
			if (name is null) {
				throw new ArgumentNullException (nameof (name));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			if (types is null) {
				throw new ArgumentNullException (nameof (types));
			}
			return Add (id, new string[] { name }, null, permissionLevel, typeof (T), null, methodName, types);
		}
		public TextCommand<PermissionLevel> Add<T> (string id, string name, object tag, PermissionLevel permissionLevel, string methodName, params Type[] types) {
			if (name is null) {
				throw new ArgumentNullException (nameof (name));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			if (types is null) {
				throw new ArgumentNullException (nameof (types));
			}
			return Add (id, new string[] { name }, tag, permissionLevel, typeof (T), null, methodName, types);
		}
		public TextCommand<PermissionLevel> Add (string id, string[] names, PermissionLevel permissionLevel, object instance, string methodName) {
			if (id is null) {
				throw new ArgumentNullException (nameof (id));
			}
			if (names is null) {
				throw new ArgumentNullException (nameof (names));
			}
			if (instance is null) {
				throw new ArgumentNullException (nameof (instance));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			return Add (id, names, null, permissionLevel, instance.GetType (), instance, methodName);
		}
		public TextCommand<PermissionLevel> Add (string id, string[] names, object tag, PermissionLevel permissionLevel, object instance, string methodName) {
			if (id is null) {
				throw new ArgumentNullException (nameof (id));
			}
			if (names is null) {
				throw new ArgumentNullException (nameof (names));
			}
			if (instance is null) {
				throw new ArgumentNullException (nameof (instance));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			return Add (id, names, tag, permissionLevel, instance.GetType (), instance, methodName);
		}
		public TextCommand<PermissionLevel> Add<T> (string id, string[] names, PermissionLevel permissionLevel, string methodName) {
			if (names is null) {
				throw new ArgumentNullException (nameof (names));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			return Add (id, names, null, permissionLevel, typeof (T), null, methodName);
		}
		public TextCommand<PermissionLevel> Add<T> (string id, string[] names, object tag, PermissionLevel permissionLevel, string methodName) {
			if (names is null) {
				throw new ArgumentNullException (nameof (names));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			return Add (id, names, tag, permissionLevel, typeof (T), null, methodName);
		}
		public TextCommand<PermissionLevel> Add (string id, string name, PermissionLevel permissionLevel, object instance, string methodName) {
			if (name is null) {
				throw new ArgumentNullException (nameof (name));
			}
			if (instance is null) {
				throw new ArgumentNullException (nameof (instance));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			return Add (id, new string[] { name }, null, permissionLevel, instance.GetType (), instance, methodName);
		}
		public TextCommand<PermissionLevel> Add (string id, string name, object tag, PermissionLevel permissionLevel, object instance, string methodName) {
			if (name is null) {
				throw new ArgumentNullException (nameof (name));
			}
			if (instance is null) {
				throw new ArgumentNullException (nameof (instance));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			return Add (id, new string[] { name }, tag, permissionLevel, instance.GetType (), instance, methodName);
		}
		public TextCommand<PermissionLevel> Add<T> (string id, string name, PermissionLevel permissionLevel, string methodName) {
			if (name is null) {
				throw new ArgumentNullException (nameof (name));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			return Add (id, new string[] { name }, null, permissionLevel, typeof (T), null, methodName);
		}
		public TextCommand<PermissionLevel> Add<T> (string id, string name, object tag, PermissionLevel permissionLevel, string methodName) {
			if (name is null) {
				throw new ArgumentNullException (nameof (name));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			return Add (id, new string[] { name }, tag, permissionLevel, typeof (T), null, methodName);
		}

		public TextCommand<PermissionLevel> Get (string id) {
			if (id is null) {
				throw new ArgumentNullException (nameof (id));
			}
			TextCommand<PermissionLevel> textCommand = null;
			ReaderWriterLockHelper.Read ((ref Dictionary<string, TextCommand<PermissionLevel>> commands) => {
				commands.TryGetValue (id, out textCommand);
			});
			return textCommand;
		}

		public void ForEach (Action<TextCommand<PermissionLevel>> action) {
			ReaderWriterLockHelper.Read ((ref Dictionary<string, TextCommand<PermissionLevel>> commands) => {
				foreach (var command in commands.Values) {
					action (command);
				}
			});
		}

		public void Clear () {
			ReaderWriterLockHelper.Write ((ref Dictionary<string, TextCommand<PermissionLevel>> commands) => {
				commands.Clear ();
			});
		}

		public void Execute (string text, PermissionLevel permissionLevel) {
			if (text is null) {
				throw new ArgumentNullException (nameof (text));
			}
			Execute (text, default, permissionLevel);
		}
		public void Execute (string text, Sender sender, PermissionLevel permissionLevel) {
			if (text is null) {
				throw new ArgumentNullException (nameof (text));
			}
			using (TextTokenizer<TextCommandTokenType> textTokenizer = new TextTokenizer<TextCommandTokenType> (
					new StringReader (text),
					TextCommandTokenType.End,
					TextCommandTokenType.String,
					TextCommandTokenType.Integer,
					TextCommandTokenType.Decimal,
					TextCommandTokenType.String,
					true
			)) {
				textTokenizer.AddKeyword ("真", TextCommandTokenType.Bool, true);
				textTokenizer.AddKeyword ("假", TextCommandTokenType.Bool, false);
				textTokenizer.AddKeyword ("true", TextCommandTokenType.Bool, true);
				textTokenizer.AddKeyword ("false", TextCommandTokenType.Bool, false);
				string name = null;
				List<TextTokenizerToken<TextCommandTokenType>> parameters = new List<TextTokenizerToken<TextCommandTokenType>> ();
				while (textTokenizer.MoveNext ()) {
					if (name is null) {
						name = textTokenizer.Current;
						continue;
					}
					parameters.Add (textTokenizer.Current);
				}
				ReaderWriterLockHelper.Read ((ref Dictionary<string, TextCommand<PermissionLevel>> commands) => {
					foreach (TextCommand<PermissionLevel> command in commands.Values) {
						if (!TextCommandApi.Contains (command.Names, name, IgnoreCase)) {
							continue;
						}
						int parameterCount = command.Parameters?.Length ?? 0;
						if (parameters.Count < parameterCount) {
							continue;
						}
						if (parameters.Count > parameterCount && !command.IsParams) {
							continue;
						}
						if (!MatchParameterType (command, parameters)) {
							continue;
						}
						object[] values = new object[command.Parameters.Length + Offset];
						for (int i = -Offset; i < command.Parameters.Length; i++) {
							if (i < 0) {
								values[i + Offset] = sender;
								continue;
							}
							if (command.IsParams && i == command.Parameters.Length - 1) {
								Array array = Array.CreateInstance (command.Parameters[i].ArrayType, parameters.Count - command.Parameters.Length + 1);
								for (int n = 0; n < array.Length; n++) {
									array.SetValue (Convert.ChangeType (parameters[i + n].Value, command.Parameters[i].ArrayType), n);
								}
								values[i + Offset] = array;
								continue;
							}
							values[i + Offset] = Convert.ChangeType (parameters[i].Value, command.Parameters[i].ParameterInfo.ParameterType);
						}
						if (Convert.ToInt32 (permissionLevel) < Convert.ToInt32 (command.PermissionLevel)) {
							PermissionError?.Invoke (sender);
							break;
						}
						command.MethodInfo.Invoke (command.Instance, values);
						break;
					}
				});
			}
		}

		TextCommand<PermissionLevel> Add (string id, string[] names, object tag, PermissionLevel permissionLevel, Type type, object instance, string methodName, params Type[] types) {
			if (id is null) {
				throw new ArgumentNullException (nameof (id));
			}
			if (names is null) {
				throw new ArgumentNullException (nameof (names));
			}
			if (type is null) {
				throw new ArgumentNullException (nameof (type));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			if (types is null) {
				throw new ArgumentNullException (nameof (types));
			}
			Array.Resize (ref types, types.Length + 1);
			Array.Copy (types, 0, types, 1, types.Length - 1);
			types[0] = typeof (Sender);
			MethodInfo methodInfo = type.GetMethod (methodName, types);
			return Add (id, names, tag, permissionLevel, instance, methodInfo);
		}
		TextCommand<PermissionLevel> Add (string id, string[] names, object tag, PermissionLevel permissionLevel, Type type, object instance, string methodName) {
			if (id is null) {
				throw new ArgumentNullException (nameof (id));
			}
			if (names is null) {
				throw new ArgumentNullException (nameof (names));
			}
			if (type is null) {
				throw new ArgumentNullException (nameof (type));
			}
			if (methodName is null) {
				throw new ArgumentNullException (nameof (methodName));
			}
			MethodInfo methodInfo = type.GetMethod (methodName);
			return Add (id, names, tag, permissionLevel, instance, methodInfo);
		}
		TextCommand<PermissionLevel> Add (string id, string[] names, object tag, PermissionLevel permissionLevel, object instance, MethodInfo methodInfo) {
			if (id is null) {
				throw new ArgumentNullException (nameof (id));
			}
			if (names is null) {
				throw new ArgumentNullException (nameof (names));
			}
			if (methodInfo is null) {
				throw new ArgumentNullException (nameof (methodInfo));
			}
			ParameterInfo[] parameterInfos = methodInfo.GetParameters ();
			List<TextCommandParameter> parameters = new List<TextCommandParameter> (parameterInfos.Length - Offset);
			bool isParams = false;
			if (parameterInfos.Length > Offset) {
				for (int i = Offset; i < parameterInfos.Length; i++) {
					TextCommandParameter parameter = new TextCommandParameter {
						Type = TextCommandApi.GetTokenType (parameterInfos[i].ParameterType),
						ParameterInfo = parameterInfos[i]
					};
					if (i == parameterInfos.Length - 1) {
						if (parameterInfos[i].ParameterType.IsArray) {
							parameter.ArrayType = parameterInfos[i].ParameterType.GetElementType ();
							isParams = true;
						}
					}
					parameters.Add (parameter);
				}
			}
			var command = new TextCommand<PermissionLevel> (names, tag, permissionLevel, instance, methodInfo, parameters.ToArray (), isParams);
			ReaderWriterLockHelper.Write ((ref Dictionary<string, TextCommand<PermissionLevel>> commands) => {
				commands[id] = command;

			});
			return command;
		}

		bool MatchParameterType (TextCommand<PermissionLevel> command, List<TextTokenizerToken<TextCommandTokenType>> parameters) {
			if (command is null) {
				throw new ArgumentNullException (nameof (command));
			}
			if (parameters is null) {
				throw new ArgumentNullException (nameof (parameters));
			}
			for (int i = 0; i < command.Parameters.Length; i++) {
				if (parameters[i].Type != command.Parameters[i].Type) {
					return false;
				}
			}
			return true;
		}

	}

}