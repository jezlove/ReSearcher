/*
	C# "ISearchResultBranchCollection.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;

namespace ReSearcher {

	public interface ISearchResultBranchCollection :
		ISearchResult {

		IEnumerable<ISearchResultBranch> branches { get; }

	}

}