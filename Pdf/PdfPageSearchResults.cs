/*
	C# "PdfPageSearchResults.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ReSearcher.Pdf {

	public sealed class PdfPageSearchResults :
		ISearchResultBranch {

		public String name {
			get { return(number.ToString()); }
		}
		public int number { get; private set; }
		public PdfFileSearchResults parent { get; internal set; }
		public ISearchResultMatchCollection matchCollection { get; private set; }

		public PdfPageSearchResults(int number, ISearchResultMatchCollection matchCollection) {
			this.number = number;
			this.matchCollection = matchCollection;
		}

		public IEnumerable<XNode> express() {
			yield return(
				new XElement("li",
					new XAttribute("class", "pdfPage"),
					new XElement("span",
						new XAttribute("class", "opener"),
						new XAttribute("onclick", String.Format("javascript:requestOpenPdfFileToPage('{0}', '{1}');", number, parent.fileInfo.FullName.Replace('\\', '/'))),
						new XElement("em", String.Format("Page {0}", number))
					),
					new XElement("div", (null == matchCollection) ? null : matchCollection.express())
				)
			);
		}

	}

}