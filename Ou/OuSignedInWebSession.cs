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
					using(new DebugIndentation()) {
						xmlDocument = cookieAwareWebClient.postFormDataReturnXmlDocument(
							signInUriString,
							new NameValueCollection()
								.with("username", username)
								.with("password", password)
								.with("Proceed1", "Sign in")
								.with("FromURL", "")
						);
						Debug.WriteLine("Cookies collected:");
						CookieCollection cookieCollection = cookieContainer.GetCookies(signInUri);
						using(new DebugIndentation()) {
							foreach(Cookie cookie in cookieCollection) {
								Debug.WriteLine("{0} = {1}", cookie.Name, cookie.Value);
							}
						}

						// SAMS001C = OFromURL=&Selected= seems to indicate failure to sign-in

						if(null == cookieCollection["SAMS001C"]) {
							Debug.WriteLine("Signed-in!");
							return(new OuSignedInWebSession(cookieContainer, cookieAwareWebClient));
						}
						else {
							Debug.WriteLine("Sign-in failed!");
							return(null);
						}
					}
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
					using(new DebugIndentation()) {
						xmlDocument = cookieAwareWebClient.getXmlDocument(signOutUriString);
						Debug.WriteLine("Signed-out!");
						signedOut = true;
						return(signedOut);
					}
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
				using(new DebugIndentation()) {
					XmlDocument xmlDocument = cookieAwareWebClient.getXmlDocument(uri);
					Debug.WriteLine("Downloaded!");
					return(xmlDocument);
				}
			}

			public XmlDocument get(Uri uri) {
				Debug.WriteLine("Getting: {0}", uri);
				using(new DebugIndentation()) {
					XmlDocument xmlDocument = cookieAwareWebClient.getXmlDocument(uri);
					Debug.WriteLine("Downloaded!");
					return(xmlDocument);
				}
			}

		#endregion

		#region hitting

			public Uri hit(Uri uri) {
				Debug.WriteLine("Hitting: {0}", uri);
				using(new DebugIndentation()) {
					cookieAwareWebClient.webRequestMethod = WebRequestMethods.Http.Head;
					{
						cookieAwareWebClient.DownloadString(uri);
						// ignore returned string
					}
					cookieAwareWebClient.webRequestMethod = null;
					Debug.WriteLine("Resolved: {0}", cookieAwareWebClient.responseUri);
					return(cookieAwareWebClient.responseUri);
				}
			}

		#endregion

		#region downloading

			public Boolean downloadFile(Uri uri, String filePath) {
				try {
					Debug.WriteLine("Downloading: {0} to {1}", uri, filePath);
					using(new DebugIndentation()) {
						cookieAwareWebClient.DownloadFile(uri, filePath);
						Debug.WriteLine("Downloaded!");
					}
					return(true);
				}
				catch(WebException webException) {
					if(HttpStatusCode.NotFound == ((HttpWebResponse)(webException.Response)).StatusCode) {
						Console.Error.WriteLine("Error 404 (NOT FOUND): ", uri);
					}
					else {
						Console.Error.WriteLine("Error, exception: {0}", webException);
					}
					return(false);
				}
			}

		#endregion

		#region user-agent

			private const String userAgentString = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";

		#endregion

	}

}