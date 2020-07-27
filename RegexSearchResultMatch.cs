/*
	C# "RegexSearchResultMatch.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ReSearcher {

	public sealed class RegexSearchResultMatch :
		ISearchResultMatch {

		public  String value {
			get {
				return(match.Value);
			}
		}

		public Match match { get; private set; }

		public RegexSearchResultMatch(Match match) {
			this.match = match;
		}

		public IEnumerable<XNode> express() {
			yield return(
				new XElement("li", 
					new XElement("code", value)
				)
			);
		}

	}

}