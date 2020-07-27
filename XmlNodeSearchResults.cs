/*
	C# "XmlNodeSearchResults.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ReSearcher {

	public sealed class XmlNodeSearchResults :
		ISearchResultBranch {

		public String name { get; private set; }
		public ISearchResultMatchCollection matchCollection { get; private set; }

		public XmlNodeSearchResults(String name, ISearchResultMatchCollection matchCollection) {
			this.name = name;
			this.matchCollection = matchCollection;
		}

		public IEnumerable<XNode> express() {
			yield return(
				new XElement("li",
					new XAttribute("class", "xmlNode"),
					new XElement("code", name),
					new XElement("div", matchCollection.express())
				)
			);
		}

	}

}