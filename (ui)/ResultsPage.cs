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

		public ResultsPage(XDocument resultsPageXDocument) :
			this() {
			display(resultsPageXDocument);
		}

		private void onOutputViewDocumentCompleted(Object sender, WebBrowserDocumentCompletedEventArgs webBrowserDocumentCompletedEventArgs) {
			WebBrowser webBrowser = sender as WebBrowser;
			webBrowser.Document.Window.Error += onOutputViewDocumentWindowError;
		}

		private void onOutputViewDocumentWindowError(Object sender, HtmlElementErrorEventArgs htmlElementErrorEventArgs) {
			htmlElementErrorEventArgs.Handled = true;
		}

		public void display(XDocument resultsPageXDocument) {
			displayBox.display(resultsPageXDocument);
		}

	}

}