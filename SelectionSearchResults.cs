/*
	C# "SelectionSearchResults.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ReSearcher {

	public sealed class SelectionSearchResults :
		ISearchResultBranchCollection {

		public IEnumerable<ISearchResultBranch> branches { get; private set; }

		public SelectionSearchResults(IEnumerable<ISearchResultBranch> branches) {
			this.branches = branches;
		}

		public IEnumerable<XNode> express() {
			yield return(
				new XElement("ul", branches.expressAll().ToArray())
			);
		}

	}

}