/*
	C# "OuDownloader.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

namespace ReSearcher.Ou {

    public class OuDownloader :
		IDownloader {

		private OuSignedInWebSession ouSignedInWebSession;

		public OuDownloader(OuSignedInWebSession ouSignedInWebSession) {
			this.ouSignedInWebSession = ouSignedInWebSession;
		}

		public IEnumerable<IDownloadableResourceFileCollection> enumerateResourceFileCollections(Func<Boolean> cancellationRequestedChecker, TextWriter logTextWriter) {
			OuStudentWebModuleResourceFileFinder ouStudentWebModuleResourceFileFinder = new OuStudentWebModuleResourceFileFinder(ouSignedInWebSession, cancellationRequestedChecker, logTextWriter);
			ouStudentWebModuleResourceFileFinder.visit();
			return(ouStudentWebModuleResourceFileFinder.findings.Select(e => new DownloadableResourceFileCollection(e.Key.shortName, e.Value)));
		}

		public Boolean download(IDownloadableResourceFile downloadableResourceFile, String filePath) {
			return(ouSignedInWebSession.downloadFile(downloadableResourceFile.downloadUri, filePath));
		}

		// ---

		public static IEnumerable<IDownloadableResourceFileCollection> enumerateResourceFileCollections(String studentUsername, String studentPassword, Func<Boolean> cancellationRequestedChecker, TextWriter logTextWriter) {
			try {

				XmlDocument signedInXmlDocument;
				OuSignedInWebSession ouSignedInWebSession = OuSignedInWebSession.signIn(studentUsername, studentPassword, out signedInXmlDocument);
				if(null == ouSignedInWebSession || null == signedInXmlDocument) {
					Console.Error.WriteLine("Error: could not sign-in student: {0}", studentUsername);
					return(Enumerable.Empty<IDownloadableResourceFileCollection>());
				}
				signedInXmlDocument.preserve("SignedIn.xml");
				using(ouSignedInWebSession) {

					// note: using block will attempt to sign-out if an exception is thrown

					OuDownloader ouDownloader = new OuDownloader(ouSignedInWebSession);

					IEnumerable<IDownloadableResourceFileCollection> findings = ouDownloader.enumerateResourceFileCollections(cancellationRequestedChecker, logTextWriter);

					// attempt to sign-out normally:

					XmlDocument signedOutXmlDocument;
					ouSignedInWebSession.signOut(out signedOutXmlDocument);
					if(null == signedOutXmlDocument) {
						Console.Error.WriteLine("Error: could not sign-out");
					}
					else {
						signedOutXmlDocument.preserve("SignedOut.xml");
					}

					return(findings);
				}

			}
			catch(Exception exception) {
				Console.Error.WriteLine("Error: exception: {0}", exception);
				return(Enumerable.Empty<IDownloadableResourceFileCollection>());
			}
		}

	}

}