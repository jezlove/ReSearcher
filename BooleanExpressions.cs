/*
	C# "BooleanExpressions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace ReSearcher {

    public static class BooleanExpressions {

		// TODO: add support for exact literals: term OR "term OR term"
		// TODO: add support for brackets: term OR (term OR term)

		public static IEnumerable<String[]> bifurcated(IEnumerable<String> tokens) {
			IList<String> operands = new List<String>();
			foreach(String token in tokens) {
				if("OR" == token) {
					if(0 != operands.Count) {
						yield return(operands.ToArray());
						operands.Clear();
					}
					continue;
				}
				if("AND" == token) {
					continue;
				}

				// TODO: NOT and nested AND / OR

				operands.Add(token);
			}
			if(0 != operands.Count) {
				yield return(operands.ToArray());
			}
		}

		public static String combined(IEnumerable<String[]> thisStringArrayIEnumerable) {
			return(String.Format("({0})", String.Join(@"|", thisStringArrayIEnumerable.Select(a => SimpleExpressions.joined(a)))));
		}

	}

}