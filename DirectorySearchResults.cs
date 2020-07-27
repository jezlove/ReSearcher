/*
	C# "DirectorySearchResults.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace ReSearcher {

	public sealed class DirectorySearchResults :
		ISearchResultBranch,
		ISearchResultBranchCollection {

		public String name {
			get {
				return(directoryInfo.Name);
			}
		}

		public DirectoryInfo directoryInfo { get; private set; }

		public IEnumerable<ISearchResultBranch> branches { get; private set; }

		public DirectorySearchResults(DirectoryInfo directoryInfo, IEnumerable<ISearchResultBranch> branches) {
			this.directoryInfo = directoryInfo;
			this.branches = branches;
		}

		public IEnumerable<XNode> express() {
			yield return(
				new XElement("li",
					new XAttribute("class", "directory"),
					new XElement("span",
						new XAttribute("class", "opener"),
						new XAttribute("onclick", String.Format("javascript:requestOpenDirectory('{0}');", directoryInfo.FullName.Replace('\\', '/'))),
						new XElement("code", name)
					),
					new XElement("ul", branches.expressAll().ToArray())
				)
			);
		}

	}

}