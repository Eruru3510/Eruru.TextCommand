using System;
using System.Collections.Generic;
using System.IO;
using Eruru.TextTokenizer;

namespace Eruru.TextCommand {

	public delegate void TextCommandAction<in T1, in T2> (T1 arg1, T2 arg2);

	static class TextCommandApi {

		public static bool Parse (TextReader textReader, out string name, out List<TextTokenizerToken<TextCommandTokenType>> parameters) {
			using (TextTokenizer<TextCommandTokenType> tokenizer = new TextTokenizer<TextCommandTokenType> (
				textReader,
				TextCommandTokenType.End,
				TextCommandTokenType.Unknown,
				TextCommandTokenType.Integer,
				TextCommandTokenType.Decimal,
				TextCommandTokenType.String,
				true
			)) {
				name = null;
				parameters = new List<TextTokenizerToken<TextCommandTokenType>> ();
				while (tokenizer.MoveNext ()) {
					if (name is null) {
						name = tokenizer.Current;
						continue;
					}
					parameters.Add (tokenizer.Current);
				}
				if (name is null) {
					return false;
				}
				return true;
			}
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
					throw new NotImplementedException ();
			}
		}

	}

}