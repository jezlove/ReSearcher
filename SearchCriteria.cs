/*
	C# "SearchCriteria.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace ReSearcher {

    public class SearchCriteria :
		ISearchCriteria {

		public IEnumerable<FileSystemInfo> fileSystemInfos { get; set; }
		public String filter { get; set; }
		public XPathExpression xpath { get; set; }
		public SearchType searchType { get; set; }
		public Regex regex { get; set; }

	}

}