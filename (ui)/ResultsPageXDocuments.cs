/*
	C# "ResultsPageXDocuments.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace ReSearcher {

    public static class ResultsPageXDocuments {

		public static XDocument toXDocument(this IEnumerable<ISearchResult> thisISearchResultIEnumerable) {
			return(
				new XDocument(
					new XDocumentType("html", "-//W3C//DTD XHTML 1.0 Strict//EN", "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd", null),
					new XElement("html",
						new XElement("head",
							new XElement("meta",
								new XAttribute("charset", "UTF-8")
							),
							new XElement("style", escapedXCData(css)),
							new XElement("script", escapedXCData(js))
						),
						new XElement("body",
							new XElement("div",
								thisISearchResultIEnumerable.expressAll().ToArray()
							)
						)
					)
				)
			);
		}

		private static Object[] escapedXCData(Object data) {
			return(new Object[] {"/*", new XCData(String.Format("*/{0}/*", data)), "*/"});
		}

		// note: cannot use a:href, must use span.onclick due to navigation being turned off in WebBrowser control

		private const String css = @"

			body {
				font-family: 'Arial', sans-serif;
			}

			code {
				font-family: 'Consolas', 'Courier New', monospace;
			}

			.opener {
				color: blue;
				cursor: hand;
				text-decoration: underline;
			}

		";

		// note: these are the only points of contact with the javascript 'external' object

		private const String js = @"

			function requestOpenDirectory(directoryPath) {
				window.external.openDirectory(directoryPath);
			}

			function requestOpenFile(filePath) {
				window.external.openFile(filePath);
			}

			function requestOpenEpubFile(epubFilePath) {
				window.external.openEpubFile(epubFilePath);
			}

			function requestOpenEpubFileToSection(title, epubFilePath) {
				window.external.openEpubFileToSection(title, epubFilePath);
			}

			function requestOpenWordFile(wordFilePath) {
				window.external.openWordFile(wordFilePath);
			}

			function requestOpenPdfFile(pdfFilePath) {
				window.external.openPdfFile(pdfFilePath);
			}

			function requestOpenPdfFileToPage(pageNumber, pdfFilePath) {
				window.external.openPdfFileToPage(pageNumber, pdfFilePath);
			}

		";

	}

}