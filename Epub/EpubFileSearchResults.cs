/*
	C# "EpubFileSearchResults.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace ReSearcher.Epub {

	public sealed class EpubFileSearchResults :
		FileSearchResults,
		ISearchResultBranchCollection {

		public IEnumerable<ISearchResultBranch> branches { get; private set; }

		public EpubFileSearchResults(FileInfo fileInfo, IEnumerable<EpubEntrySearchResults> branches) :
			base(fileInfo) {
			this.branches = branches;
			foreach(EpubEntrySearchResults branch in branches) {
				branch.parent = this;
			}
		}

		public override IEnumerable<XNode> express() {
			yield return(
				new XElement("li",
					new XAttribute("class", "epubFile"),
					new XElement("span",
						new XAttribute("class", "opener"),
						new XAttribute("onclick", String.Format("javascript:requestOpenEpubFile('{0}');", fileInfo.FullName.Replace('\\', '/'))),
						new XElement("code", name)
					),
					new XElement("ol", branches.expressAll().ToArray())
				)
			);
		}

	}

}