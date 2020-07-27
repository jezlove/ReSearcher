/*
	C# "ResultsPage.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace ReSearcher {

    public class ResultsPage :
		TabPage {

		protected WebBrowser displayBox { get; private set; }
		protected ResultsPageObjectForScripting resultsPageObjectForScripting { get; private set; }

		public ResultsPage() {
			Text = "Results";
			Padding = new Padding(10);
			resultsPageObjectForScripting = new ResultsPageObjectForScripting(this);
			this.appendControls(
				new Panel() { Dock = DockStyle.Fill, BorderStyle = BorderStyle.Fixed3D }.withControls(
					displayBox = new WebBrowser() {
						Dock = DockStyle.Fill,
						ObjectForScripting = resultsPageObjectForScripting,
						AllowNavigation = false,
						AllowWebBrowserDrop = false,
#if !DEBUG
						IsWebBrowserContextMenuEnabled = false,
#endif
					}
				)
			);
			displayBox.DocumentCompleted += onOutputViewDocumentCompleted;
		}

		public ResultsPage(IEnumerable<ISearchResult> results) :
			this() {
			display(results);
		}

		private void onOutputViewDocumentCompleted(Object sender, WebBrowserDocumentCompletedEventArgs webBrowserDocumentCompletedEventArgs) {
			WebBrowser webBrowser = sender as WebBrowser;
			webBrowser.Document.Window.Error += onOutputViewDocumentWindowError;
		}

		private void onOutputViewDocumentWindowError(Object sender, HtmlElementErrorEventArgs htmlElementErrorEventArgs) {
			htmlElementErrorEventArgs.Handled = true;
		}

		public void display(IEnumerable<ISearchResult> results) {
			displayBox.display(documentAround(results));
		}

		protected static XDocument documentAround(IEnumerable<ISearchResult> results) {
			return(
				new XDocument(
					new XDocumentType("html", "-//W3C//DTD XHTML 1.0 Strict//EN", "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd", null),
					new XElement("html",
						new XElement("head",
							new XElement("style", escapedXCData(css)),
							new XElement("script", escapedXCData(js))
						),
						new XElement("body",
							new XElement("div",
								results.expressAll().ToArray()
							)
						)
					)
				)
			);
		}

		public static Object[] escapedXCData(Object data) {
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