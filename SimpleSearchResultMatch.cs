/*
	C# "SimpleSearchResultMatch.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ReSearcher {

	public sealed class SimpleSearchResultMatch :
		ISearchResultMatch {

		public String value { get; private set; }

		public SimpleSearchResultMatch(String value) {
			this.value = value;
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