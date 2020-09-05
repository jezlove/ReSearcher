/*
	C# "XmlDocumentExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace ReSearcher {

    public static class XmlDocumentExtensions {

		[Conditional("DEBUG")]
		public static void preserve(this XmlDocument thisXmlDocument, String xmlFileName) {
/*
			String directoryPath = "cache";
			if(!Directory.Exists(directoryPath)) {
				Directory.CreateDirectory(directoryPath);
			}
			String xmlFilePath = Path.Combine(directoryPath, xmlFileName);
			thisXmlDocument.Save(xmlFilePath);
*/
		}

	}

}