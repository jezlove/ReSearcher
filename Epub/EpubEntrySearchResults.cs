/*
	C# "EpubEntrySearchResults.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ReSearcher.Epub {

	public sealed class EpubEntrySearchResults :
		ISearchResultBranch {

		public String name { get; private set; }
		public String title { get; private set; }
		public EpubFileSearchResults parent { get; internal set; }
		public ISearchResultMatchCollection matchCollection { get; private set; }
		public IEnumerable<ISearchResultBranch> branches { get; private set; }

		public EpubEntrySearchResults(String name, String title, ISearchResultMatchCollection matchCollection) {
			this.name = name;
			this.title = title;
			this.matchCollection = matchCollection;
			this.branches = Enumerable.Empty<ISearchResultBranch>();
		}

		public EpubEntrySearchResults(String name, String title, IEnumerable<ISearchResultBranch> branches) {
			this.name = name;
			this.title = title;
			this.matchCollection = null;
			this.branches = branches;
		}

		public IEnumerable<XNode> express() {
			yield return(
				new XElement("li",
					new XAttribute("class", "epubEntry"),
					new XElement("span",
						new XAttribute("class", "opener"),
						new XAttribute("onclick", String.Format("javascript:requestOpenEpubFileToSection('{0}', '{1}');", title, parent.fileInfo.FullName.Replace('\\', '/'))),
						new XElement("em", title ?? " ")
					),
#if DEBUG
					new XElement("code", name),
#endif
					new XElement("div", (null == matchCollection) ? null : matchCollection.express()),
					new XElement("ol", branches.expressAll().ToArray())
				)
			);
		}

	}

}