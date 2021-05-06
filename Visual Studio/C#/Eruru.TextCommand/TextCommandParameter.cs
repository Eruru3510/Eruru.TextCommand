using System;
using System.Reflection;

namespace Eruru.TextCommand {

	public class TextCommandParameter {

		public ParameterInfo ParameterInfo { get; }
		public TextCommandTokenType Type { get; }
		public Type ElementType { get; }

		public TextCommandParameter (ParameterInfo parameterInfo) {
			if (parameterInfo is null) {
				throw new ArgumentNullException (nameof (parameterInfo));
			}
			ParameterInfo = parameterInfo;
			Type = TextCommandApi.GetTokenType (parameterInfo.ParameterType);
			ElementType = parameterInfo.ParameterType.GetElementType ();
		}

	}

}