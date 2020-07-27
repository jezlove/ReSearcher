/*
	C# "CookieContainerExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Net;

namespace ReSearcher {

	public static class CookieContainerExtensions {

		public static void collectFrom(this CookieContainer thisCookieContainer, WebResponse webResponse) {
			HttpWebResponse httpWebResponse = webResponse as HttpWebResponse;
			if(null != httpWebResponse) {
				thisCookieContainer.collectFrom(httpWebResponse);
			}
		}

		public static void collectFrom(this CookieContainer thisCookieContainer, HttpWebResponse httpWebResponse) {
			thisCookieContainer.Add(httpWebResponse.Cookies);
		}

	}

}