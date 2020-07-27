/*
	C# "OuSignInForm.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Xml;
using System.Windows.Forms;

using ReSearcher.Ou;

namespace ReSearcher {

    public class OuSignInForm :
		AbstractSignInForm {

		private OuSignedInWebSession ouSignedInWebSession;

		protected override Boolean trySignIn() {
			XmlDocument signedInXmlDocument;
			ouSignedInWebSession = OuSignedInWebSession.signIn(username, password, out signedInXmlDocument);
			signedInXmlDocument.preserve("SignedIn.xml");
			if(null == ouSignedInWebSession) {
				Console.Error.WriteLine("Error: could not sign-in student: {0}", username);
				return(false);
			}
			return(true);
		}

		public static OuSignedInWebSession signIn(IWin32Window ownerWindow) {
			using(OuSignInForm ouSignInForm = new OuSignInForm() {
				username = ProgramSettings.ouUsername,
				password = ProgramSettings.ouPassword
			}) {
				if(DialogResult.OK != ouSignInForm.ShowDialog(ownerWindow)) {
					return(null);
				}
				if(ouSignInForm.rememberMeChecked) {
					ProgramSettings.ouUsername = ouSignInForm.username;
					ProgramSettings.ouPassword = ouSignInForm.password;
				}
				else {
					ProgramSettings.ouUsername = null;
					ProgramSettings.ouPassword = null;
				}
				return(ouSignInForm.ouSignedInWebSession);
			}
		}

	}

}