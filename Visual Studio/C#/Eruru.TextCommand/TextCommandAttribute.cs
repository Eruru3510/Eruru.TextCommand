using System;

namespace Eruru.TextCommand {

	[AttributeUsage (AttributeTargets.Method)]
	public class TextCommandAttribute : Attribute {

		public string[] Names { get; set; }
		public int PermissionLevel { get; set; }
		public object Tag { get; set; }

		public TextCommandAttribute (string name, object tag, int permissionLevel) {
			if (name is null) {
				throw new ArgumentNullException (nameof (name));
			}
			Names = new string[] { name };
			PermissionLevel = permissionLevel;
			Tag = tag;
		}
		public TextCommandAttribute (string name, object tag) : this (name, tag, default) {

		}
		public TextCommandAttribute (string name, int permissionLevel) : this (name, default, permissionLevel) {

		}
		public TextCommandAttribute (string name) : this (name, default, default (int)) {

		}
		public TextCommandAttribute () {

		}
		public TextCommandAttribute (string[] names, object tag, int permissionLevel) {
			if (names is null) {
				throw new ArgumentNullException (nameof (names));
			}
			Names = names;
			PermissionLevel = permissionLevel;
			Tag = tag;
		}
		public TextCommandAttribute (string[] names, object tag) : this (names, tag, default) {

		}
		public TextCommandAttribute (string[] names, int permissionLevel) : this (names, default, permissionLevel) {

		}
		public TextCommandAttribute (params string[] names) : this (names, default, default) {

		}

	}

}