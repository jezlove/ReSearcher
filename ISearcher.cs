/*
	C# "ISearcher.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace ReSearcher {

    public interface ISearcher {

		SelectionSearchResults search(IEnumerable<FileSystemInfo> fileSystemInfos, Func<Boolean> cancellationRequestedChecker, TextWriter log);

	}

}