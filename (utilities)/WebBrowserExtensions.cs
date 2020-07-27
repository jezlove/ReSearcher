/*
	C# "WebBrowserExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace ReSearcher {

	public static class WebBrowserExtensions {

		#region displaying

			public static void display(this WebBrowser thisWebBrowser, XNode xNode) {
				thisWebBrowser.DocumentText = leafNodeRegex.Replace(xNode.stringify(), "<$1$2></$1>");
			}

			// note: IE has issues with <div/> <em/> etc.

			private static readonly Regex leafNodeRegex = new Regex(@"<\b([\w\-\.:]+)\b([^>]*)/>", RegexOptions.Compiled);

		#endregion

	}

}