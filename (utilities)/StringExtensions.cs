/*
	C# "StringExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace ReSearcher {

	public static class StringExtensions {

		#region replacing

			public static String replaceAll(this String thisString, IEnumerable<Char> replaceCharacters, Char replacementCharacter) {
				return(null == thisString ? thisString : new String(thisString.Select(c => (replaceCharacters.Contains(c) ? replacementCharacter : c)).ToArray()));
			}

			public static String replaceAllWhitespace(this String thisString, Char replacementCharacter) {
				return(null == thisString ? thisString : new String(thisString.Select(c => (Char.IsWhiteSpace(c) ? replacementCharacter : c)).ToArray()));
			}

		#endregion

	}

}