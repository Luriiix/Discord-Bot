using ParameterInfo = System.Reflection.ParameterInfo;

namespace Discord_Bot;

public static class Parser {
	private static Dictionary<Type, Func<string, object>> _parser = [];

	private static void Add<T>(Func<string, object> parser) {
		_parser.Add(typeof(T), parser);
	}

	private static void Parse(ParameterInfo parameter, string arg, out object o) {
		_parser.TryGetValue(parameter.ParameterType, out var parser);
		if (parser == null) throw new NullReferenceException($"Parser for type {parameter.ParameterType} not found");

		o = parser(arg);

		if (o == null) throw new NullReferenceException($"Could not parse {arg} to type {parameter.ParameterType}");
	}

	public static void AddAllParsers() {
		Add<int>(arg => int.Parse(arg));
		Add<string>(arg => arg);
	}

	public static List<object> ParseMessageToParameters(string message, ParameterInfo[] parameters) {
		var argList = new List<object>();
		var messageArray = message.Split(" ");

		for (var i = 1; i < messageArray.Length; i++) {
			if (!messageArray[i].StartsWith('(')) continue;

			var closingIndex = FindClosingParenthesis(messageArray, i);
			if (closingIndex == -1) continue;

			var combinedString = string.Join(' ', messageArray[i..(closingIndex + 1)]);

			messageArray = UpdateMessageArray(messageArray, i, closingIndex, combinedString);

			break;
		}

		for (var i = 1; i < parameters.Length; i++) {
			Parse(parameters[i], messageArray[i], out var obj);
			argList.Add(obj);
		}
		
		return argList;
	}

	private static string[] UpdateMessageArray(
		string[] messageArray, int startIndex, int closingIndex, string combinedString
	) {
		var list = new List<string>();

		for (var k = 0; k < messageArray.Length; k++) {
			if (k < startIndex) {
				list.Add(messageArray[k]);
			} else if (k == startIndex) {
				list.Add(combinedString);
				k = closingIndex;
			} else if (k > closingIndex) {
				list.Add(messageArray[k]);
			}
		}

		return list.ToArray();
	}

	private static int FindClosingParenthesis(string[] messageArray, int startIndex) {
		for (var j = startIndex; j < messageArray.Length; j++) {
			if (messageArray[j].EndsWith(')')) {
				return j;
			}
		}

		return -1; // Kein schließendes Klammer gefunden
	}
}
