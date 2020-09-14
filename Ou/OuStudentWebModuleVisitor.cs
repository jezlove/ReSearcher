/*
	C# "OuStudentWebModuleVisitor.cs"
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

	public abstract class OuStudentWebModuleVisitor :
		IVisitor {

		protected readonly OuSignedInWebSession ouSignedInWebSession;
		protected readonly Func<Boolean> cancellationRequestedChecker;
		protected readonly IndentedWriter log;

		public OuStudentWebModuleVisitor(OuSignedInWebSession ouSignedInWebSession, Func<Boolean> cancellationRequestedChecker, TextWriter logTextWriter) {
			this.ouSignedInWebSession = ouSignedInWebSession;
			this.cancellationRequestedChecker = cancellationRequestedChecker;
			this.log = new IndentedWriter(logTextWriter, "  ");
		}

		#region visiting

			private const String ouStudentUriString = "https://msds.open.ac.uk/students/";

			public virtual void visit() {
				if(cancellationRequestedChecker()) return;
				try {
					XmlDocument xmlDocument = ouSignedInWebSession.get(ouStudentUriString);
					if(null == xmlDocument) {
						log.WriteLine("Error: failed to download: {0}", ouStudentUriString);
						return;
					}
					xmlDocument.preserve("OuStudent.xml");
					visit(xmlDocument);
				}
				catch(Exception exception) {
					log.WriteLine("Unable to access: {0}, exception: {1}", ouStudentUriString, exception);
				}
			}

			public virtual void visit(XmlDocument xmlDocument) {
				if(cancellationRequestedChecker()) return;
				OuStudent ouStudent = OuStudent.parse(xmlDocument);
				if(null == ouStudent) {
					onCouldNotParseStudentHome();
					return;
				}
				visit(ouStudent, xmlDocument);
			}

			public virtual void visit(OuStudent ouStudent, XmlDocument xmlDocument) {
				if(cancellationRequestedChecker()) return;
				IList<OuStudentModule> ouStudentModules = ouStudent.modules.OrderByDescending(m => m.presentation).ToList();

				#if DEBUG

					foreach(OuStudentModule ouStudentModule in ouStudentModules) {
						Debug.WriteLine("Found module: {0}", ouStudentModule);
					}

				#endif

				using(new WritingIndentation(log)) {
					foreach(OuStudentModule ouStudentModule in ouStudentModules) {
						visitStudentModule(ouStudentModule);
					}
				}

			}

			protected virtual void onCouldNotParseStudentHome() {
				log.WriteLine("Error: could not parse student home");
			}

		#endregion

		#region visiting-student-modules

			private const String moduleHomePatternUriString = "https://learn2.open.ac.uk/course/view.php?name={0}";

			protected virtual void visitStudentModule(OuStudentModule ouStudentModule) {
				if(cancellationRequestedChecker()) return;
				log.WriteLine("Inspecting module: {0}", ouStudentModule);
				String uri = String.Format(moduleHomePatternUriString, ouStudentModule.shortName);
				try {
					XmlDocument xmlDocument = ouSignedInWebSession.get(uri);
					if(null == xmlDocument) {
						log.WriteLine("Error: failed to download: {0}", uri);
						return;
					}
					xmlDocument.preserve(ouStudentModule.name + ".xml");
					visitStudentModule(ouStudentModule, xmlDocument);
				}
				catch(Exception exception) {
					log.WriteLine("Unable to access module: {0}, exception: {1}", ouStudentModule.shortName, exception);
				}
			}

			protected abstract void visitStudentModule(OuStudentModule ouStudentModule, XmlDocument xmlDocument);

		#endregion

	}

}