/*
	C# "OuStudentWebModuleResourceFileFinder.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace ReSearcher.Ou {

	public class OuStudentWebModuleResourceFileFinder :
		OuStudentWebModuleResourceVisitor {

		public readonly IDictionary<OuStudentModule, List<IDownloadableResourceFile>> findings = new Dictionary<OuStudentModule, List<IDownloadableResourceFile>>();

		public OuStudentWebModuleResourceFileFinder(OuSignedInWebSession ouSignedInWebSession, Func<Boolean> cancellationRequestedChecker, TextWriter logTextWriter) :
			base(ouSignedInWebSession, cancellationRequestedChecker, logTextWriter) {
		}

		#region visiting-student-module-resource-downloads-in-format

			protected override void visitStudentModuleResourceDownloadsInFormat(OuStudentModule ouStudentModule, String formatName, XmlDocument xmlDocument) {
				if(cancellationRequestedChecker()) return;
				XmlNodeList trXmlNodeList = xmlDocument.SelectNodes(".//table/tbody/tr");
				if(0 == trXmlNodeList.Count) {
					onNoFilesAvailable(ouStudentModule, formatName);
					return;
				}
				using(new WritingIndentation(log)) {
					foreach(XmlNode trXmlNode in trXmlNodeList) {
						if(cancellationRequestedChecker()) return;
						visitStudentModuleResourceDownloadFile(ouStudentModule, formatName, trXmlNode);
					}
				}
			}

			protected virtual void onNoFilesAvailable(OuStudentModule ouStudentModule, String formatName) {
				Console.Out.WriteLine("Note: no files available");
			}

		#endregion

		#region visiting-student-module-resource-download-file

			protected virtual void visitStudentModuleResourceDownloadFile(OuStudentModule ouStudentModule, String formatName, XmlNode trXmlNode) {
				if(cancellationRequestedChecker()) return;
				XmlNode uriAXmlNode = trXmlNode.SelectSingleNode("./td[2]//a[@href]");
				if(null == uriAXmlNode) {
					log.WriteLine("Error: link missing");
					return;
				}
				String name = uriAXmlNode.InnerText.Trim();
				if(String.IsNullOrWhiteSpace(name)) {
					log.WriteLine("Error: name missing");
					return;
				}
				XmlNode sizeTdXmlNode = trXmlNode.SelectSingleNode("./td[3]");
				String sizeString = (null == sizeTdXmlNode) ? null : sizeTdXmlNode.InnerText.Trim();
				if(String.IsNullOrWhiteSpace(sizeString)) {
					log.WriteLine("Error: size missing");
					return;
				}
				long size;
				if(!SiByteMultiples.tryParse(sizeString, out size)) {
					log.WriteLine("Error: size invalid");
					return;
				}
				XmlNode locationTdXmlNode = trXmlNode.SelectSingleNode("./td[4]");
				String location = (null == locationTdXmlNode) ? null : locationTdXmlNode.InnerText.Trim();
				if(String.IsNullOrWhiteSpace(location)) {
					// note: not all entries have locations
					location = null;
				}

				Uri downloadUri = new Uri(uriAXmlNode.Attributes["href"].Value);

				if(!hasFinalResourceDownloadExtension(downloadUri)) {
					downloadUri = ouSignedInWebSession.hit(downloadUri);
				}

				findings.getValueOrNew(ouStudentModule).Add(
					new DownloadableResourceFile(
						downloadUri: downloadUri,
						name: name,
						type: formatName,
						size: size,
						location: location
					)
				);
			}

			private static Boolean hasFinalResourceDownloadExtension(Uri uri) {
				String extension = Path.GetExtension(uri.AbsolutePath);
				if(null == extension) {
					return(false);
				}
				extension = extension.ToLowerInvariant();
				if(".php" == extension) {
					return(false);
				}
				return(true);
			}

		#endregion

	}

}