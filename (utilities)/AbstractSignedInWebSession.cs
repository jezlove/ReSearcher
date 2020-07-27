/*
	C# "AbstractSignedInWebSession.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Net;

namespace ReSearcher {

	public abstract class AbstractSignedInWebSession :
		IDisposable {

		protected readonly CookieContainer cookieContainer;

		protected readonly CookieAwareWebClient cookieAwareWebClient;

		public Boolean signedOut { get; protected set; }

		protected AbstractSignedInWebSession(CookieContainer cookieContainer, CookieAwareWebClient cookieAwareWebClient) {
			this.cookieContainer = cookieContainer;
			this.cookieAwareWebClient = cookieAwareWebClient;
			signedOut = false;
		}

		public abstract Boolean signOut();

		public void Dispose() {
			if(!signedOut) {
				signOut();
			}
		}

	}

}