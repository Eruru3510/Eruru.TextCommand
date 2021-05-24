using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Eruru.ReaderWriterLockHelper;
using Eruru.TextTokenizer;

namespace Eruru.TextCommand {

	public class TextCommandSystem<Sender, PermissionLevel> where PermissionLevel : IComparable {

		public bool IgnoreCase { get; set; } = false;
		public bool MatchParameterType { get; set; } = true;
		public bool EnablePermissionLevel { get; set; } = true;
		public event TextCommandAction<Sender, TextCommand<PermissionLevel>> OnPermissionInsufficient;

		readonly ReaderWriterLockHelper<Dictionary<string, TextCommand<PermissionLevel>>> ReaderWriterLockHelper =
			new ReaderWriterLockHelper<Dictionary<string, TextCommand<PermissionLevel>>> (new Dictionary<string, TextCommand<PermissionLevel>> ());

		internal const int ParameterOffset = 1;

		public void Register<T> () {
			ReaderWriterLockHelper.Write (commands => {
				foreach (MethodInfo methodInfo in typeof (T).GetMethods (BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)) {
					TextCommandAttribute attribute = methodInfo.GetAttribute<TextCommandAttribute> ();
					if (attribute is null) {
						continue;
					}
					PermissionLevel permissionLevel = (PermissionLevel)Enum.ToObject (typeof (PermissionLevel), attribute.PermissionLevel);
					string id = methodInfo.ToString ();
					commands[id] = new TextCommand<PermissionLevel> (id, attribute.Names, attribute.Tag, permissionLevel, methodInfo);
				}
			});
		}

		public void Add (string id, string name, object tag, PermissionLevel permissionLevel, Delegate action) {
			if (id is null) {
				throw new ArgumentNullException (nameof (id));
			}
			if (name is null) {
				throw new ArgumentNullException (nameof (name));
			}
			if (action is null) {
				throw new ArgumentNullException (nameof (action));
			}
			ReaderWriterLockHelper.Write (commands => {
				commands[id] = new TextCommand<PermissionLevel> (id, name, tag, permissionLevel, action);
			});
		}
		public void Add (string id, string name, PermissionLevel permissionLevel, Delegate action) {
			Add (id, name, default, permissionLevel, action);
		}
		public void Add (string id, string name, object tag, Delegate action) {
			Add (id, name, tag, default, action);
		}
		public void Add (string id, string name, Delegate action) {
			Add (id, name, default, default, action);
		}
		public void Add (string id, string[] names, object tag, PermissionLevel permissionLevel, Delegate action) {
			if (id is null) {
				throw new ArgumentNullException (nameof (id));
			}
			if (names is null) {
				throw new ArgumentNullException (nameof (names));
			}
			if (action is null) {
				throw new ArgumentNullException (nameof (action));
			}
			ReaderWriterLockHelper.Write (commands => {
				commands[id] = new TextCommand<PermissionLevel> (id, names, tag, permissionLevel, action);
			});
		}
		public void Add (string id, string[] names, PermissionLevel permissionLevel, Delegate action) {
			Add (id, names, default, permissionLevel, action);
		}
		public void Add (string id, string[] names, object tag, Delegate action) {
			Add (id, names, tag, default, action);
		}
		public void Add (string id, string[] names, Delegate action) {
			Add (id, names, default, default, action);
		}
		public void Add (string name, object tag, PermissionLevel permissionLevel, Delegate action) {
			Add (action.Method.ToString (), name, tag, permissionLevel, action);
		}
		public void Add (string name, PermissionLevel permissionLevel, Delegate action) {
			Add (action.Method.ToString (), name, default, permissionLevel, action);
		}
		public void Add (string name, object tag, Delegate action) {
			Add (action.Method.ToString (), name, tag, default, action);
		}
		public void Add (string name, Delegate action) {
			Add (action.Method.ToString (), name, default, default, action);
		}
		public void Add (string[] names, object tag, PermissionLevel permissionLevel, Delegate action) {
			Add (action.Method.ToString (), names, tag, permissionLevel, action);
		}
		public void Add (string[] names, PermissionLevel permissionLevel, Delegate action) {
			Add (action.Method.ToString (), names, default, permissionLevel, action);
		}
		public void Add (string[] names, object tag, Delegate action) {
			Add (action.Method.ToString (), names, tag, default, action);
		}
		public void Add (string[] names, Delegate action) {
			Add (action.Method.ToString (), names, default, default, action);
		}

		public bool Execute<T> (string text, bool ignoreCase, Sender sender, PermissionLevel permissionLevel, out T returnValue) {
			if (text is null) {
				throw new ArgumentNullException (nameof (text));
			}
			if (!TextCommandApi.Parse (new StringReader (text), out string name, out List<TextTokenizerToken<TextCommandTokenType>> parameters)) {
				returnValue = default;
				return false;
			}
			T insideReturnValue = default;
			bool executed = false;
			ReaderWriterLockHelper.Read (commands => {
				foreach (var command in commands.Values) {
					if (!TextCommandApi.Contains (command.Names, name, ignoreCase)) {
						continue;
					}
					if (parameters.Count < command.Parameters.Length) {
						continue;
					}
					if (!command.IsParams && parameters.Count > command.Parameters.Length) {
						continue;
					}
					if (MatchParameterType && !Match (command.Parameters, parameters)) {
						continue;
					}
					object[] values = new object[command.Parameters.Length + ParameterOffset];
					try {
						int index = -ParameterOffset;
						for (int i = 0; i < values.Length; i++, index++) {
							if (i < ParameterOffset) {
								switch (i) {
									case 0:
										values[i] = sender;
										break;
									default:
										throw new NotImplementedException ();
								}
								continue;
							}
							if (i == values.Length - 1 && command.IsParams) {
								Array array = Array.CreateInstance (command.Parameters[index].ElementType, parameters.Count - command.Parameters.Length + 1);
								for (int n = 0; n < array.Length; n++) {
									array.SetValue (Convert.ChangeType (parameters[index + n].Value, command.Parameters[index].ElementType), n);
								}
								values[i] = array;
								continue;
							}
							values[i] = Convert.ChangeType (parameters[index].Value, command.Parameters[index].ParameterInfo.ParameterType);
						}
					} catch {
						continue;
					}
					if (permissionLevel.CompareTo (command.PermissionLevel) == -1) {
						OnPermissionInsufficient?.Invoke (sender, command);
						return;
					}
					insideReturnValue = (T)command.MethodInfo.Invoke (command.Instance, values);
					executed = true;
					return;
				}
			});
			returnValue = insideReturnValue;
			return executed;
		}
		public bool Execute<T> (string text, Sender sender, PermissionLevel permissionLevel, out T returnValue) {
			return Execute (text, IgnoreCase, sender, permissionLevel, out returnValue);
		}
		public bool Execute<T> (string text, bool ignoreCase, Sender sender, out T returnValue) {
			return Execute (text, ignoreCase, sender, default, out returnValue);
		}
		public bool Execute<T> (string text, Sender sender, out T returnValue) {
			return Execute (text, IgnoreCase, sender, default, out returnValue);
		}
		public bool Execute<T> (string text, bool ignoreCase, PermissionLevel permissionLevel, out T returnValue) {
			return Execute (text, ignoreCase, default, permissionLevel, out returnValue);
		}
		public bool Execute<T> (string text, PermissionLevel permissionLevel, out T returnValue) {
			return Execute (text, IgnoreCase, default, permissionLevel, out returnValue);
		}
		public bool Execute<T> (string text, bool ignoreCase, out T returnValue) {
			return Execute (text, ignoreCase, default, default, out returnValue);
		}
		public bool Execute<T> (string text, out T returnValue) {
			return Execute (text, IgnoreCase, default, default, out returnValue);
		}
		public bool Execute (string text, bool ignoreCase, Sender sender, PermissionLevel permissionLevel) {
			return Execute<object> (text, ignoreCase, sender, permissionLevel, out _);
		}
		public bool Execute (string text, Sender sender, PermissionLevel permissionLevel) {
			return Execute<object> (text, IgnoreCase, sender, permissionLevel, out _);
		}
		public bool Execute (string text, bool ignoreCase, Sender sender) {
			return Execute<object> (text, ignoreCase, sender, default, out _);
		}
		public bool Execute (string text, Sender sender) {
			return Execute<object> (text, IgnoreCase, sender, default, out _);
		}
		public bool Execute (string text, bool ignoreCase, PermissionLevel permissionLevel) {
			return Execute<object> (text, ignoreCase, default, permissionLevel, out _);
		}
		public bool Execute (string text, PermissionLevel permissionLevel) {
			return Execute<object> (text, IgnoreCase, default, permissionLevel, out _);
		}
		public bool Execute (string text, bool ignoreCase) {
			return Execute<object> (text, ignoreCase, default, default, out _);
		}
		public bool Execute (string text) {
			return Execute<object> (text, IgnoreCase, default, default, out _);
		}

		public void ForEach (Action<TextCommand<PermissionLevel>> action) {
			if (action is null) {
				throw new ArgumentNullException (nameof (action));
			}
			ReaderWriterLockHelper.Read (commands => {
				foreach (var command in commands.Values) {
					action (command);
				}
			});
		}

		bool Match (TextCommandParameter[] parameterTypes, List<TextTokenizerToken<TextCommandTokenType>> parameters) {
			if (parameterTypes is null) {
				throw new ArgumentNullException (nameof (parameterTypes));
			}
			if (parameters is null) {
				throw new ArgumentNullException (nameof (parameters));
			}
			for (int i = 0; i < parameterTypes.Length; i++) {
				if (parameterTypes[i].Type != parameters[i].Type) {
					return false;
				}
			}
			return true;
		}

	}

}