/*
	C# "XNodeExtensions.cs"
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

	public static class XNodeExtensions {

		#region stringifying

			public static String stringify(this XNode xNode) {
				using(StringWriter stringWriter = new StringWriter()) {
					XmlWriterSettings xmlWriterSettings = new XmlWriterSettings() {
						OmitXmlDeclaration = true,

						#if DEBUG

							Indent = true,
							IndentChars = ("\t"),

						#endif

					};
					using(XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings)) {
						xNode.WriteTo(xmlWriter);
					}
					return(stringWriter.ToString());
				}
			}

		#endregion

	}

}