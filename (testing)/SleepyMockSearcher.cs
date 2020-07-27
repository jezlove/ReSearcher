/*
	C# "SleepyMockSearcher.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace ReSearcher {

    internal class SleepyMockSearcher :
		MockSearcher {

		private int searchFileDuration = 1;

		public SleepyMockSearcher(String filter = Filters.any) :
			base(filter) {
		}

		#region searching-files

			protected override FileSearchResults searchFile(FileInfo fileInfo) {
				FileSearchResults fileSearchResults = base.searchFile(fileInfo);
				Thread.Sleep(searchFileDuration);
				return(fileSearchResults);
			}

		#endregion

	}

}