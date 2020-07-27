/*
	C# "OuSignedInWebSession.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Specialized;
using System.Net;
using System.Xml;
using System.Diagnostics;

namespace ReSearcher.Ou {

	public sealed class OuSignedInWebSession :
		AbstractSignedInWebSession {

		private OuSignedInWebSession(CookieContainer cookieContainer, CookieAwareWebClient cookieAwareWebClient) :
			base(cookieContainer, cookieAwareWebClient) {
		}

		#region signing-in

			private const String signInUriString = "https://msds.open.ac.uk/signon/SAMS001A.aspx";
			private static readonly Uri signInUri = new Uri(signInUriString);

			public static OuSignedInWebSession signIn(String username, String password, out XmlDocument xmlDocument) {
				xmlDocument = null;
				try {
					CookieContainer cookieContainer = new CookieContainer();
					CookieAwareWebClient cookieAwareWebClient = new CookieAwareWebClient(cookieContainer);
					cookieAwareWebClient.Headers.withUserAgent(userAgentString);
					Debug.WriteLine("Attempting sign-in...");
					xmlDocument = cookieAwareWebClient.postFormDataReturnXmlDocument(
						signInUriString,
						new NameValueCollection()
							.with("username", username)
							.with("password", password)
							.with("Proceed1", "Sign in")
							.with("FromURL", "")
					);
					Debug.WriteLine("Cookies collected:");
					using(new DebugIndentation()) {
						foreach(Cookie cookie in cookieContainer.GetCookies(signInUri)) {
							Debug.WriteLine("{0} = {1}", cookie.Name, cookie.Value);
						}
					}
					Debug.WriteLine("Signed-in!");
					return(new OuSignedInWebSession(cookieContainer, cookieAwareWebClient));
				}
				catch(Exception exception) {
					Console.Error.WriteLine("Error: exception: {0}", exception);
					return(null);
				}
			}

			public static OuSignedInWebSession signIn(String username, String password) {
				XmlDocument xmlDocument;
				return(signIn(username, password, out xmlDocument));
			}

		#endregion

		#region signing-out

			private const String signOutUriString = "https://msds.open.ac.uk/signon/samsoff.aspx";

			public Boolean signOut(out XmlDocument xmlDocument) {
				xmlDocument = null;
				try {
					Debug.WriteLine("Attempting sign-out...");
					xmlDocument = cookieAwareWebClient.getXmlDocument(signOutUriString);
					Debug.WriteLine("Signed-out!");
					signedOut = true;
					return(signedOut);
				}
				catch(Exception exception) {
					Console.Error.WriteLine("Error: exception: {0}", exception);
					return(signedOut);
				}
			}

			public override Boolean signOut() {
				XmlDocument xmlDocument;
				return(signOut(out xmlDocument));
			}

		#endregion

		#region getting

			public XmlDocument get(String uri) {
				Debug.WriteLine("Getting: {0}", (Object)(uri));
				XmlDocument xmlDocument = cookieAwareWebClient.getXmlDocument(uri);
				Debug.WriteLine("Downloaded");
				return(xmlDocument);
			}

			public XmlDocument get(Uri uri) {
				Debug.WriteLine("Getting: {0}", uri);
				XmlDocument xmlDocument = cookieAwareWebClient.getXmlDocument(uri);
				Debug.WriteLine("Downloaded");
				return(xmlDocument);
			}

		#endregion

		#region hitting

			public Uri hit(Uri uri) {
				Debug.WriteLine("Hitting: {0}", uri);
				cookieAwareWebClient.webRequestMethod = WebRequestMethods.Http.Head;
				{
					cookieAwareWebClient.DownloadString(uri);
					// ignore returned string
				}
				cookieAwareWebClient.webRequestMethod = null;
				Debug.WriteLine("Resolved: {0}", cookieAwareWebClient.responseUri);
				return(cookieAwareWebClient.responseUri);
			}

		#endregion

		#region hitting

			public void downloadFile(Uri uri, String filePath) {
				Debug.WriteLine("Downloading: {0} to {1}", uri, filePath);
				cookieAwareWebClient.DownloadFile(uri, filePath);
				Debug.WriteLine("Downloaded");
			}

		#endregion

		#region user-agent

			private const String userAgentString = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";

		#endregion

	}

}