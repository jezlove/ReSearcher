/*
	C# "ISearchCriteria.cs"
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

    public interface ISearchCriteria {

		IEnumerable<FileSystemInfo> fileSystemInfos { get; }
		String filter { get; }
		XPathExpression xpath { get; }
		SearchType searchType { get; }
		Regex regex { get; }

	}

}