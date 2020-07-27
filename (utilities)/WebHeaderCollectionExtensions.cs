/*
	C# "WebHeaderCollectionExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;

namespace ReSearcher {

	public static partial class WebHeaderCollectionExtensions {

		#region user-agent

			public static TWebHeaderCollection withUserAgent<TWebHeaderCollection>(this TWebHeaderCollection thisWebHeaderCollection, String userAgentString) where TWebHeaderCollection : WebHeaderCollection {
				thisWebHeaderCollection.Add(HttpRequestHeader.UserAgent, userAgentString);
				return(thisWebHeaderCollection);
			}

		#endregion

		#region content-type

			public static TWebHeaderCollection withContentType<TWebHeaderCollection>(this TWebHeaderCollection thisWebHeaderCollection, String contentTypeString) where TWebHeaderCollection : WebHeaderCollection {
				thisWebHeaderCollection.Add(HttpRequestHeader.ContentType, contentTypeString);
				return(thisWebHeaderCollection);
			}

		#endregion

	}

}