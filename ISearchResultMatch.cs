/*
	C# "ISearchResultMatch.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;

namespace ReSearcher {

	public interface ISearchResultMatch :
		ISearchResult {

		String value { get; }

	}

}