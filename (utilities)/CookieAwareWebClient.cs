/*
	C# "CookieAwareWebClient.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Net;

namespace ReSearcher {

	public class CookieAwareWebClient :
		WebClient {

		public String webRequestMethod { get; set; }

		public Uri responseUri { get; protected set; }

		public CookieContainer cookieContainer { get; protected set; }

		public CookieAwareWebClient() :
			this(new CookieContainer()) {
		}

		public CookieAwareWebClient(CookieContainer cookieContainer) {
			this.cookieContainer = cookieContainer;
		}

		protected override WebRequest GetWebRequest(Uri uri) {
			WebRequest webRequest = base.GetWebRequest(uri);
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if(null != httpWebRequest) {
				httpWebRequest.CookieContainer = cookieContainer;
			}
			if(!String.IsNullOrEmpty(webRequestMethod)) {
                webRequest.Method = webRequestMethod;
			}
			return(webRequest);
		}

		protected override WebResponse GetWebResponse(WebRequest webRequest) {
			WebResponse webResponse = base.GetWebResponse(webRequest);
			responseUri = webResponse.ResponseUri;
			cookieContainer.collectFrom(webResponse);
			return(webResponse);
		}

		protected override WebResponse GetWebResponse(WebRequest webRequest, IAsyncResult asyncResult) {
			WebResponse webResponse = base.GetWebResponse(webRequest, asyncResult);
			responseUri = webResponse.ResponseUri;
			cookieContainer.collectFrom(webResponse);
			return(webResponse);
		}

	}

}