/*
	C# "WebClientExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Collections.Specialized;

namespace ReSearcher {

	public static partial class WebClientExtensions {

		#region getting

			public static XmlDocument getXmlDocument(this WebClient thisWebClient, String uriString) {
				using(Stream stream = thisWebClient.OpenRead(uriString)) {
					return(SgmlDocuments.readHtmlAsXmlDocument(stream));
				}
			}

			public static XmlDocument getXmlDocument(this WebClient thisWebClient, Uri uri) {
				using(Stream stream = thisWebClient.OpenRead(uri)) {
					return(SgmlDocuments.readHtmlAsXmlDocument(stream));
				}
			}

		#endregion

		#region posting

			public static String postFormDataReturnString(this WebClient thisWebClient, String uriString, NameValueCollection nameValueCollection) {
				thisWebClient.Headers.withContentType(formDataContentType);
				return(thisWebClient.UploadValues(uriString, "POST", nameValueCollection).toUtf8String());
			}

			public static XmlDocument postFormDataReturnXmlDocument(this WebClient thisWebClient, String uriString, NameValueCollection nameValueCollection) {
				String responseString = thisWebClient.postFormDataReturnString(uriString, nameValueCollection);
				return(SgmlDocuments.readHtmlAsXmlDocument(responseString));
			}

			private const String formDataContentType = "application/x-www-form-urlencoded";

		#endregion

	}

}