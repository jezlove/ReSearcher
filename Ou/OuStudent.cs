/*
	C# "OuStudent.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace ReSearcher.Ou {

	public sealed class OuStudent {

		public readonly IList<OuStudentModule> modules;

		private OuStudent(IList<OuStudentModule> modules) {
			this.modules = modules;
		}

		#region parsing

			public static OuStudent parse(XmlDocument xmlDocument) {

				XmlNode scriptXmlNode = xmlDocument.SelectSingleNode(".//div[@id='int-content']/script[1]");
				if(null == scriptXmlNode) {
					Console.Error.WriteLine("Error: could not locate script");
					return(null);
				}

				String scriptText = scriptXmlNode.InnerText;

				Match codeArrayMatch = codeArrayRegex.Match(scriptText);
				Match presentationArrayMatch = presentationArrayRegex.Match(scriptText);
				Match typeArrayMatch = typeArrayRegex.Match(scriptText);
				Match titleArrayMatch = titleArrayRegex.Match(scriptText);

				if(!codeArrayMatch.Success || !presentationArrayMatch.Success || !typeArrayMatch.Success || !titleArrayMatch.Success) {
					Console.Error.WriteLine("Error: regexes did not match script: {0}, {1}, {2}, {3}", codeArrayMatch.Success, presentationArrayMatch.Success, typeArrayMatch.Success, titleArrayMatch.Success);
					return(null);
				}

				CaptureCollection codes = codeArrayMatch.Groups[2].Captures;
				CaptureCollection presentations = presentationArrayMatch.Groups[2].Captures;
				CaptureCollection types = typeArrayMatch.Groups[2].Captures;
				CaptureCollection titles = titleArrayMatch.Groups[2].Captures;

				int count = codes.Count;
				if(presentations.Count != count || types.Count != count || titles.Count != count) {
					Console.Error.WriteLine("Error: inconsistent number of matches: {0}, {1}, {2}, {3}", codes.Count, presentations.Count, types.Count, titles.Count);
					return(null);
				}

				IList<OuStudentModule> modules = new List<OuStudentModule>();
				for(int i = 0; i < count; i++) {
					int type;
					if(!Int32.TryParse(types[i].Value, out type)) {
						type = -1;
					}
					modules.Add(
						new OuStudentModule(
							moduleCode: delimiterRegex.Replace(codes[i].Value, String.Empty),
							presentation: delimiterRegex.Replace(presentations[i].Value, String.Empty),
							title: delimiterRegex.Replace(titles[i].Value, String.Empty),
							type: type
						)
					);
				}

				return(new OuStudent(modules));
			}

			private const String patternString = @"
				\b(?:var|let|const)\b \s+ \b({0})\b \s* = \s* \[
					\s*
					(?:
						((?:\d+|""[^""]+""|'[^']+'))
						\s*
						,?
						\s*
					)*
				\] \s* ;
			";

			private static readonly Regex codeArrayRegex = new Regex(String.Format(patternString, "ioe_course"), RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
			private static readonly Regex presentationArrayRegex = new Regex(String.Format(patternString, "ioe_pres"), RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
			private static readonly Regex typeArrayRegex = new Regex(String.Format(patternString, "ioe_crsetype"), RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
			private static readonly Regex titleArrayRegex = new Regex(String.Format(patternString, "ioe_crsetitle"), RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

			private static readonly Regex delimiterRegex = new Regex("[\"']", RegexOptions.Compiled);

		#endregion

	}

}