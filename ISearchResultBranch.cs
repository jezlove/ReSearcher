/*
	C# "ISearchResultBranch.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;

namespace ReSearcher {

	public interface ISearchResultBranch :
		ISearchResult {

		String name { get; }

	}

}