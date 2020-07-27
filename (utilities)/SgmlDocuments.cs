/*
	C# "SgmlDocuments.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Sgml;

namespace ReSearcher {

	public static class SgmlDocuments {

		#region reading

			public static XmlDocument readHtmlAsXmlDocument(String text) {
				return(doReadHtmlAsXmlDocument(text));
			}

			public static XmlDocument readHtmlAsXmlDocument(Stream stream) {
				using(StreamReader streamReader = new StreamReader(stream)) {
					return(readHtmlAsXmlDocument(streamReader));
				}
			}

			public static XmlDocument readHtmlAsXmlDocument(TextReader textReader) {
				return(doReadHtmlAsXmlDocument(textReader.ReadToEnd()));
			}

			private static XmlDocument doReadHtmlAsXmlDocument(String htmlString) {

				// note: xmlns needs to be stripped manually before parsing
				htmlString = xmlnsRegex.Replace(htmlString, String.Empty);

				using(StringReader stringReader = new StringReader(htmlString)) {
					SgmlReader sgmlReader = new SgmlReader() {
						DocType = "HTML",
						IgnoreDtd = true,
						WhitespaceHandling = WhitespaceHandling.All,
						CaseFolding = CaseFolding.ToLower,
						InputStream = stringReader
					};
					XmlDocument xmlDocument = new XmlDocument() {
						PreserveWhitespace = true,
						XmlResolver = null
					};
					xmlDocument.Load(sgmlReader);
					return(xmlDocument);
				}
			}

			private static Regex xmlnsRegex = new Regex(@"\s+\bxmlns\b(?::\w+)?\s*=\s*(?:""[^""]*""|'[^']*')", RegexOptions.Compiled);

		#endregion

	}

}