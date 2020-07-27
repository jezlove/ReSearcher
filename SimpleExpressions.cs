/*
	C# "SimpleExpressions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace ReSearcher {

    public static class SimpleExpressions {

		public static IEnumerable<String> tokenised(String thisString) {
			StringBuilder stringBuilder = new StringBuilder();
			foreach(Char character in thisString) {
				if(Char.IsLetterOrDigit(character) || '_' == character || '-' == character || '\'' == character) {
					stringBuilder.Append(character);
					continue;
				}
				if(Char.IsWhiteSpace(character)) {
					if(0 != stringBuilder.Length) {
						yield return(stringBuilder.ToString());
						stringBuilder.Clear();
					}
					continue;
				}
			}
			if(0 != stringBuilder.Length) {
				yield return(stringBuilder.ToString());
			}
		}

		public static String joined(IEnumerable<String> words) {
			return(String.Join(@"\b.+\b", words));
		}

	}

}