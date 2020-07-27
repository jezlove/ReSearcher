/*
	C# "MockSearcher.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace ReSearcher {

    internal class MockSearcher :
		AbstractSearcher {

		public MockSearcher(String filter = Filters.any) :
			base(filter) {
		}

		#region searching-files

			protected override FileSearchResults searchFile(FileInfo fileInfo) {
				return(new SimpleFileSearchResults(fileInfo, new SimpleSearchResultMatchCollection(Enumerable.Empty<ISearchResultMatch>())));
			}

		#endregion

	}

}