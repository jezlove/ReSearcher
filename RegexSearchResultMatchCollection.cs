/*
	C# "RegexSearchResultMatchCollection.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ReSearcher {

	public sealed class RegexSearchResultMatchCollection :
		ISearchResultMatchCollection {

		public IEnumerable<ISearchResultMatch> matches {
			get {
				foreach(Match match in matchCollection) {
					yield return(new RegexSearchResultMatch(match));
				}
			}
		}

		public MatchCollection matchCollection { get; private set; }

		public RegexSearchResultMatchCollection(MatchCollection matchCollection) {
			this.matchCollection = matchCollection;			
		}

		public IEnumerable<XNode> express() {
			yield return(
				new XElement("ol", matches.expressAll().ToArray())
			);
		}

	}

}