/*
	C# "WordFileSearchResults.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace ReSearcher.Word {

	public sealed class WordFileSearchResults :
		FileSearchResults {

		public ISearchResultMatchCollection matchCollection { get; private set; }

		public WordFileSearchResults(FileInfo fileInfo, ISearchResultMatchCollection matchCollection) :
			base(fileInfo) {
			this.matchCollection = matchCollection;
		}

		public override IEnumerable<XNode> express() {
			yield return(
				new XElement("li",
					new XAttribute("class", "wordFile"),
					new XElement("span",
						new XAttribute("class", "opener"),
						new XAttribute("onclick", String.Format("javascript:requestOpenWordFile('{0}');", fileInfo.FullName.Replace('\\', '/'))),
						new XElement("code", name)
					),
					new XElement("div", (null == matchCollection) ? null : matchCollection.express())
				)
			);
		}

	}

}