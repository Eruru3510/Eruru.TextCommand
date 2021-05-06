using System;
using Eruru.TextCommand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1 {

	[TestClass]
	public class RegisterAndUseCommand {

		[TestMethod]
		public void TestMethod1 () {
			TextCommandSystem<object, PermissionLevel> textCommandSystem = new TextCommandSystem<object, PermissionLevel> ();
			textCommandSystem.Register<RegisterAndUseCommand> ();
			textCommandSystem.Add ("加法", (object)"给定两整数，返回总和", new Func<object, int, int, int> ((sender, a, b) => {
				return a + b;
			}));
			textCommandSystem.Execute ("加法 1.1 1.2", out float floatResult);
			Assert.IsTrue (Equals (2.3F, floatResult));
			textCommandSystem.Execute ("问候", "Jack", out string message);
			Assert.AreEqual ("Jack发来了问候", message);
			textCommandSystem.Execute ("求和 1.1 1.2 1.3", out floatResult);
			Assert.IsTrue (Equals (3.6F, floatResult));
			textCommandSystem.Execute ("加法 1 2", out int intResult);
			Assert.AreEqual (3, intResult);
			textCommandSystem.ForEach (command => {
				Console.WriteLine (command.Tag);
			});
		}

		[TextCommand ("加法", (object)"给定两数，返回总和")]
		static float Add (object sender, float a, float b) {
			return a + b;
		}

		[TextCommand ("问候")]
		static string Greet (object sender) {
			return $"{sender}发来了问候";
		}

		[TextCommand ("求和", (object)"变长参数")]
		static float Sum (object sender, params float[] values) {
			float total = 0;
			foreach (float value in values) {
				total += value;
			}
			return total;
		}

		bool Equals (float a, float b) {
			return Math.Abs (a - b) <= 0.00001F;
		}

	}

}