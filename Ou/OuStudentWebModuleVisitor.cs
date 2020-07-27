/*
	C# "OuStudentWebModuleVisitor.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Diagnostics;

namespace ReSearcher.Ou {

	public abstract class OuStudentWebModuleVisitor :
		IVisitor {

		protected readonly OuSignedInWebSession ouSignedInWebSession;

		public OuStudentWebModuleVisitor(OuSignedInWebSession ouSignedInWebSession) {
			this.ouSignedInWebSession = ouSignedInWebSession;
		}

		#region visiting

			private const String ouStudentUriString = "https://msds.open.ac.uk/students/";

			public virtual void visit() {
				XmlDocument xmlDocument = ouSignedInWebSession.get(ouStudentUriString);
				if(null == xmlDocument) {
					Console.Error.WriteLine("Error: failed to download: {0}", ouStudentUriString);
					return;
				}
				xmlDocument.preserve("OuStudent.xml");
				visit(xmlDocument);
			}

			public virtual void visit(XmlDocument xmlDocument) {
				OuStudent ouStudent = OuStudent.parse(xmlDocument);
				if(null == ouStudent) {
					onCouldNotParseStudentHome();
					return;
				}
				visit(ouStudent, xmlDocument);
			}

			public virtual void visit(OuStudent ouStudent, XmlDocument xmlDocument) {
				IList<OuStudentModule> ouStudentModules = ouStudent.modules.OrderByDescending(m => m.presentation).ToList();
				foreach(OuStudentModule ouStudentModule in ouStudentModules) {
					Debug.WriteLine("Found module: {0}", ouStudentModule);
				}
				using(new DebugIndentation()) {
					foreach(OuStudentModule ouStudentModule in ouStudentModules) {
						visitStudentModule(ouStudentModule);
					}
				}
			}

			protected virtual void onCouldNotParseStudentHome() {
				Console.Error.WriteLine("Error: could not parse student home");
			}

		#endregion

		#region visiting-student-modules

			private const String moduleHomePatternUriString = "https://learn2.open.ac.uk/course/view.php?name={0}";

			protected virtual void visitStudentModule(OuStudentModule ouStudentModule) {
				Debug.WriteLine("Visiting module: {0}", ouStudentModule);
				String uri = String.Format(moduleHomePatternUriString, ouStudentModule.shortName);
				XmlDocument xmlDocument = ouSignedInWebSession.get(uri);
				if(null == xmlDocument) {
					Console.Error.WriteLine("Error: failed to download: {0}", uri);
					return;
				}
				xmlDocument.preserve(ouStudentModule.name + ".xml");
				visitStudentModule(ouStudentModule, xmlDocument);
			}

			protected abstract void visitStudentModule(OuStudentModule ouStudentModule, XmlDocument xmlDocument);

		#endregion

	}

}