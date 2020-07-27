/*
	C# "PdfFileSearchResults.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace ReSearcher.Pdf {

	public sealed class PdfFileSearchResults :
		FileSearchResults,
		ISearchResultBranchCollection {

		public IEnumerable<ISearchResultBranch> branches { get; private set; }

		public PdfFileSearchResults(FileInfo fileInfo, IEnumerable<PdfPageSearchResults> branches) :
			base(fileInfo) {
			this.branches = branches;
			foreach(PdfPageSearchResults branch in branches) {
				branch.parent = this;
			}
		}

		public override IEnumerable<XNode> express() {
			yield return(
				new XElement("li",
					new XAttribute("class", "pdfFile"),
					new XElement("span",
						new XAttribute("class", "opener"),
						new XAttribute("onclick", String.Format("javascript:requestOpenPdfFile('{0}');", fileInfo.FullName.Replace('\\', '/'))),
						new XElement("code", name)
					),
					new XElement("ol", branches.expressAll().ToArray())
				)
			);
		}

	}

}