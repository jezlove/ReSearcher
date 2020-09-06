/*
	C# "OuStudentWebModuleResourceVisitor.cs"
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

	public abstract class OuStudentWebModuleResourceVisitor :
		OuStudentWebModuleVisitor {

		public OuStudentWebModuleResourceVisitor(OuSignedInWebSession ouSignedInWebSession, Func<Boolean> cancellationRequestedChecker, TextWriter logTextWriter) :
			base(ouSignedInWebSession, cancellationRequestedChecker, logTextWriter) {
		}

		#region visiting-student-modules

			protected override void visitStudentModule(OuStudentModule ouStudentModule, XmlDocument xmlDocument) {
				if(cancellationRequestedChecker()) return;
				XmlNode resourcesAXmlNode = xmlDocument.SelectSingleNode(".//a[normalize-space(text())='Resources' and @href]");
				if(null == resourcesAXmlNode) {
					onCouldNotLocateResourcesLink(ouStudentModule);
					return;
				}
				String resourcesUri = resourcesAXmlNode.Attributes["href"].Value;
				using(new WritingIndentation(log)) {
					visitStudentModuleResources(ouStudentModule, resourcesUri);
				}
			}

			protected virtual void onCouldNotLocateResourcesLink(OuStudentModule ouStudentModule) {
				Console.Error.WriteLine("Error: could not locate 'Resources' link");
			}

		#endregion

		#region visiting-student-module-resources

			protected virtual void visitStudentModuleResources(OuStudentModule ouStudentModule, String resourcesUri) {
				if(cancellationRequestedChecker()) return;
				log.WriteLine("Inspecting module resources");
				XmlDocument xmlDocument = ouSignedInWebSession.get(resourcesUri);
				if(null == xmlDocument) {
					Console.Error.WriteLine("Error: failed to download: {0}", resourcesUri);
					return;
				}
				xmlDocument.preserve(ouStudentModule.name + "_resources.xml");
				visitStudentModuleResources(ouStudentModule, xmlDocument);
			}

			protected virtual void visitStudentModuleResources(OuStudentModule ouStudentModule, XmlDocument xmlDocument) {
				if(cancellationRequestedChecker()) return;
				XmlNode downloadsAXmlNode = xmlDocument.SelectSingleNode(".//a[normalize-space(./span/span/span/text())='Downloads' and @href]");
				if(null == downloadsAXmlNode) {
					onCouldNotLocateDownloadsLink(ouStudentModule);
					return;
				}
				String downloadsUri = downloadsAXmlNode.Attributes["href"].Value;
				using(new WritingIndentation(log)) {
					visitStudentModuleResourceDownloads(ouStudentModule, downloadsUri);
				}
			}

			protected virtual void onCouldNotLocateDownloadsLink(OuStudentModule ouStudentModule) {
				Console.Error.WriteLine("Error: could not locate 'Downloads' link");
			}

		#endregion

		#region visiting-student-module-resource-downloads

			protected virtual void visitStudentModuleResourceDownloads(OuStudentModule ouStudentModule, String downloadsUri) {
				if(cancellationRequestedChecker()) return;
				log.WriteLine("Inspecting module resource downloads");
				XmlDocument xmlDocument = ouSignedInWebSession.get(downloadsUri);
				if(null == xmlDocument) {
					Console.Error.WriteLine("Error: failed to download: {0}", downloadsUri);
					return;
				}
				xmlDocument.preserve(ouStudentModule.name + "_resource_downloads.xml");
				visitStudentModuleResourceDownloads(ouStudentModule, xmlDocument);
			}

			protected virtual void visitStudentModuleResourceDownloads(OuStudentModule ouStudentModule, XmlDocument xmlDocument) {
				visitStudentModuleResourceDocumentDownloads(ouStudentModule, xmlDocument);
				visitStudentModuleResourceMediaDownloads(ouStudentModule, xmlDocument);
			}

			#region documents

				protected virtual void visitStudentModuleResourceDocumentDownloads(OuStudentModule ouStudentModule, XmlDocument xmlDocument) {
					if(cancellationRequestedChecker()) return;
					XmlNode documentDownloadsUlXmlNode = xmlDocument.SelectSingleNode(".//h3[normalize-space(text())='Document downloads']/following-sibling::ul");
					if(null == documentDownloadsUlXmlNode) {
						onCouldNotLocateDocumentDownloadsList(ouStudentModule);
						return;
					}
					XmlNodeList aXmlNodeList = documentDownloadsUlXmlNode.SelectNodes(".//li//a[@href]");
					if(0 == aXmlNodeList.Count) {
						onNoDocumentFormatsAvailable(ouStudentModule);
						return;
					}
					visitStudentModuleResourceDownloads(ouStudentModule, aXmlNodeList);
				}

				protected virtual void onCouldNotLocateDocumentDownloadsList(OuStudentModule ouStudentModule) {
					Console.Error.WriteLine("Warning: could not locate 'Document downloads' format list");
				}

				protected virtual void onNoDocumentFormatsAvailable(OuStudentModule ouStudentModule) {
					Console.Out.WriteLine("Note: no document formats available");
				}

			#endregion

			#region media

				protected virtual void visitStudentModuleResourceMediaDownloads(OuStudentModule ouStudentModule, XmlDocument xmlDocument) {
					if(cancellationRequestedChecker()) return;
					XmlNode mediaDownloadsUlXmlNode = xmlDocument.SelectSingleNode(".//h3[normalize-space(text())='Media downloads']/following-sibling::ul");
					if(null == mediaDownloadsUlXmlNode) {
						onCouldNotLocateMediaDownloadsList(ouStudentModule);
						return;
					}
					XmlNodeList aXmlNodeList = mediaDownloadsUlXmlNode.SelectNodes(".//li//a[@href]");
					if(0 == aXmlNodeList.Count) {
						onNoMediaFormatsAvailable(ouStudentModule);
						return;
					}
					visitStudentModuleResourceDownloads(ouStudentModule, aXmlNodeList);
				}

				protected virtual void onCouldNotLocateMediaDownloadsList(OuStudentModule ouStudentModule) {
					Console.Error.WriteLine("Warning: could not locate 'Media downloads' format list");
				}

				protected virtual void onNoMediaFormatsAvailable(OuStudentModule ouStudentModule) {
					Console.Out.WriteLine("Note: no media formats available");
				}

			#endregion

			protected virtual void visitStudentModuleResourceDownloads(OuStudentModule ouStudentModule, XmlNodeList aXmlNodeList) {
				if(cancellationRequestedChecker()) return;
				using(new WritingIndentation(log)) {
					foreach(XmlNode aXmlNode in aXmlNodeList) {
						if(cancellationRequestedChecker()) return;
						String formatUriString = aXmlNode.Attributes["href"].Value;
						String formatName = aXmlNode.InnerText.Trim();
						visitStudentModuleResourceDownloadsInFormat(ouStudentModule, formatName, formatUriString);
					}
				}
			}

		#endregion

		#region visiting-student-module-resource-downloads-in-format

			protected virtual void visitStudentModuleResourceDownloadsInFormat(OuStudentModule ouStudentModule, String formatName, String formatUriString) {
				if(cancellationRequestedChecker()) return;
				log.WriteLine("Inspecting module resource downloads in format: {0}", (Object)(formatName));
				XmlDocument xmlDocument = ouSignedInWebSession.get(formatUriString);
				if(null == xmlDocument) {
					Console.Error.WriteLine("Error: failed to download: {0}", formatUriString);
					return;
				}
				xmlDocument.preserve(ouStudentModule.name + "_resource_downloads_in_" + formatName + ".xml");
				visitStudentModuleResourceDownloadsInFormat(ouStudentModule, formatName, xmlDocument);
			}

			protected abstract void visitStudentModuleResourceDownloadsInFormat(OuStudentModule ouStudentModule, String formatName, XmlDocument xmlDocument);

		#endregion

	}

}