/*
	C# "FileSearchResults.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace ReSearcher {

	public abstract class FileSearchResults :
		ISearchResultBranch {

		public String name {
			get {
				return(fileInfo.Name);
			}
		}

		public FileInfo fileInfo { get; private set; }

		public FileSearchResults(FileInfo fileInfo) {
			this.fileInfo = fileInfo;
		}

		public abstract IEnumerable<XNode> express();

	}

}