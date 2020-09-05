/*
	C# "SophisticatedSearcher.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

using ReSearcher.Epub;

namespace ReSearcher {

    public class SophisticatedSearcher :
		AbstractMultiFormatSearcher {

		private readonly XPathExpression xpath;

		public SophisticatedSearcher(Regex regex, XPathExpression xpath, String filter = Filters.any) :
			base(regex, filter) {
			this.xpath = xpath;
		}

		#region searching-epub-documents

			protected override EpubEntrySearchResults searchEpubEntry(String entryName, Stream stream) {

				XmlDocument xmlDocument = SgmlDocuments.readHtmlAsXmlDocument(stream); // EPUB uses XHTML not HTML, but let's be forgiving
				if(null == xmlDocument) {
					Console.Error.WriteLine("Error: could not parse: {0}", entryName);
					return(null);
				}

				// TODO: switch to using XPathNavigator

				IEnumerable<Tuple<XmlNode, MatchCollection>> xmlNodeMatchCollections = xmlDocument.SelectNodes(xpath.Expression).matchWithinAll(regex);
				if(xmlNodeMatchCollections.isNullOrEmpty()) {
					return(null);
				}

				XmlNode titleXmlNode = xmlDocument.SelectSingleNode(".//title");
				String entryTitle = (null == titleXmlNode) ? null : titleXmlNode.InnerText.Trim();

				return(new EpubEntrySearchResults(entryName, entryTitle, xmlNodeMatchCollections.Select(t => new XmlNodeSearchResults(t.Item1.Name, new RegexSearchResultMatchCollection(t.Item2)))));
			}

		#endregion

	}

}