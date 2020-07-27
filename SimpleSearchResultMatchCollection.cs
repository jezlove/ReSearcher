/*
	C# "SimpleSearchResultMatchCollection.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ReSearcher {

	public sealed class SimpleSearchResultMatchCollection :
		ISearchResultMatchCollection {

		public IEnumerable<ISearchResultMatch> matches { get; private set; }

		public SimpleSearchResultMatchCollection(IEnumerable<ISearchResultMatch> matches) {
			this.matches = matches;			
		}

		public IEnumerable<XNode> express() {
			yield return(
				new XElement("ol", matches.expressAll().ToArray())
			);
		}

	}

}